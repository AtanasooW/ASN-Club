using ASNClub.Data;
using ASNClub.Data.Models.AddressModels;
using ASNClub.Data.Models.Orders;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CountyServices;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.Services.OrderServices;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Order;
using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ASNClub.Common.EntityValidationConstants;
using ASNClub.Data.Models.Product;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.AddressServices;
using ASNClub.Services.ShoppingCartServices;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.ProductServices;

namespace ASNClub.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private DbContextOptions<ASNClubDbContext> dbOptions;
        private ASNClubDbContext dbContext;
        private ICountryService countryService;
        private IShoppingCartService shoppingCartService;
        private IAddressService addressService;
        private IOrderService orderService;
        private IProductService productService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            dbOptions = new DbContextOptionsBuilder<ASNClubDbContext>()
                .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new ASNClubDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            countryService = new CountryService(dbContext);
            productService = new ProductService(dbContext);
            shoppingCartService = new ShoppingCartService(dbContext, productService);
            addressService = new AddressService(dbContext);
            orderService = new OrderService(dbContext, countryService, shoppingCartService, addressService);

            orderService = new OrderService(dbContext, countryService, shoppingCartService, addressService);
        }

        [Test]
        public async Task GetAllOrdersAsync_Should_Return_All_Orders()
        {
            // Act
            var result = await orderService.GetAllOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<MyOrderViewModel>>(result);
            Assert.AreEqual(dbContext.Orders.Count(), result.Count);
        }
        [Test]
        public async Task GetMyOrdersByIdAsync_Should_Return_Orders_For_Valid_UserId()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var orderService = new OrderService(dbContext, null, null, null);

            // Act
            var result = await orderService.GetMyOrdersByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<MyOrderViewModel>>(result);
            Assert.AreEqual(dbContext.Orders.Count(x => x.UserId == userId), result.Count);
        }
    }
}
