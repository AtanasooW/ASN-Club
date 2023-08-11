using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using ASNClub.Data;
using ASNClub.Services.AddressServices;
using ASNClub.ViewModels.Address;
using Microsoft.Extensions.DependencyInjection;
using ASNClub.Services.ProfileServices;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Data.Models.AddressModels;

namespace ASNClub.Tests
{
    [TestFixture]
    public class AddressServiceTests
    {
        private ASNClubDbContext dbContext;
        private IAddressService addressService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<ASNClubDbContext>()
                .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new ASNClubDbContext(options);

            // Seed the database for your tests
            DatabaseSeeder.SeedDatabaseAsync(dbContext, null);
            addressService = new AddressService(dbContext);

        }

        [Test]
        public async Task AddAddressAsync_Should_AddAddress()
        {
            // Arrange
            var addressViewModel = new AddressViewModel
            {
                CountryId = 1,
                City = "Sofia",
                PostalCode = "1000",
                Street1 = "Tzar Boris 3",
                StreetNumber = "57",
                IsDefault = true
            };
            var userId = Guid.NewGuid();

            // Act
            await addressService.AddAddressAsync(addressViewModel, userId);
            var savedAddress = await dbContext.Addresses.FirstOrDefaultAsync();
            var savedUserAddress = await dbContext.UsersAddresses.FirstOrDefaultAsync();

            // Assert
            Assert.IsNotNull(savedAddress);
            Assert.IsNotNull(savedUserAddress);
            Assert.AreEqual(1, savedAddress.CountryId);
            Assert.AreEqual("Sofia", savedAddress.City);
            Assert.AreEqual("1000", savedAddress.PostalCode);
            Assert.AreEqual("Tzar Boris 3", savedAddress.Street1);
            Assert.AreEqual("57", savedAddress.StreetNumber);
            Assert.IsTrue(savedAddress.IsDefault);
            Assert.AreEqual(userId, savedUserAddress.UserId);
            Assert.AreEqual(savedAddress.Id, savedUserAddress.AddressId);
        }

        [Test]
        public async Task GetShippingAddressByIdAsync_Should_ReturnShippingAddress()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Seed a user's default shipping address
            var address = new Address
            {
                Id = Guid.NewGuid(),
                CountryId = 1,
                City = "Sofia",
                PostalCode = "1000",
                Street1 = "Tzar Boris 3",
                StreetNumber = "57",
                IsDefault = true
            };
            var userAddress = new UserAddress
            {
                UserId = userId,
                AddressId = address.Id
            };
            dbContext.Addresses.Add(address);
            dbContext.UsersAddresses.Add(userAddress);
            await dbContext.SaveChangesAsync();

            // Act
            var shippingAddress = await addressService.GetShippingAddressByIdAsync(userId);

            // Assert
            Assert.IsNotNull(shippingAddress);
            Assert.AreEqual(address.Id, shippingAddress.Id);
            Assert.IsTrue(shippingAddress.IsDefault);
            Assert.AreEqual(1, shippingAddress.CountryId);
            Assert.AreEqual("Sofia", shippingAddress.City);
            Assert.AreEqual("1000", shippingAddress.PostalCode);
            Assert.AreEqual("Tzar Boris 3", shippingAddress.Street1);
            Assert.AreEqual("57", shippingAddress.StreetNumber);
        }
        [Test]
        public async Task SetDefaultAddressAsync_Should_SetDefaultAddress()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var addressId = Guid.NewGuid();
            // Seed data as needed

            // Act
            await addressService.SetDefaultAddressAsync(addressId, userId);
            var updatedAddress = await dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);

            // Assert
            Assert.IsTrue(updatedAddress.IsDefault);
            // Additional assertions as needed
        }
    }
}
