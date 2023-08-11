using ASNClub.Data;
using ASNClub.Data.Models;
using ASNClub.Services.ProfileServices;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ASNClub.Tests
{
    [TestFixture]
    public class ProfileServiceTests
    {
        private DbContextOptions<ASNClubDbContext> dbOptions;
        private ASNClubDbContext dbContext;
        private ProfileService profileService;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            dbOptions = new DbContextOptionsBuilder<ASNClubDbContext>()
                .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            dbContext = new ASNClubDbContext(dbOptions);
            dbContext.Database.EnsureCreated();

            profileService = new ProfileService(dbContext);
        }

        [Test]
        public async Task EditProfileAsync_Should_Update_Profile_With_Given_FormModel()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Vasil",
                SurnameName = "Karas",
                Email = "Vase@example.com",
                PhoneNumber = "1234567890"
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var formModel = new ProfileFormModel
            {
                Id = userId,
                FirstName = "Pesho",
                SurnameName = "Vailev",
                Email = "pesho@example.com",
                PhoneNumber = "9876543210"
            };

            // Act
            await profileService.EditProfileAsync(formModel);

            // Assert
            var editedUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            Assert.NotNull(editedUser);
            Assert.AreEqual("Pesho", editedUser.FirstName);
            Assert.AreEqual("Vailev", editedUser.SurnameName);
            Assert.AreEqual("pesho@example.com", editedUser.Email);
            Assert.AreEqual("9876543210", editedUser.PhoneNumber);
        }
        [Test]
        public async Task GetProfileByIdAsync_Should_Return_Profile_ViewModel_For_Valid_Id()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Vasil",
                SurnameName = "Karas",
                Email = "Vasil@example.com",
                PhoneNumber = "1234567890"
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Act
            var profile = await profileService.GetProfileByIdAsync(userId);

            // Assert
            Assert.NotNull(profile);
            Assert.AreEqual(userId, profile.Id);
            Assert.AreEqual("Vasil", profile.FirstName);
            Assert.AreEqual("Karas", profile.SurnameName);
            Assert.AreEqual("Vasil@example.com", profile.Email);
            Assert.AreEqual("1234567890", profile.PhoneNumber);
        }

        [Test]
        public async Task GetProfileByIdForEditAsync_Should_Return_ProfileFormModel_For_Valid_Id()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Vasil",
                SurnameName = "Karas",
                Email = "Vasil@example.com",
                PhoneNumber = "1234567890"
            };
            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            // Act
            var formModel = await profileService.GetProfileByIdForEditAsync(userId);

            // Assert
            Assert.NotNull(formModel);
            Assert.AreEqual(userId, formModel.Id);
            Assert.AreEqual("Vasil", formModel.FirstName);
            Assert.AreEqual("Karas", formModel.SurnameName);
            Assert.AreEqual("Vasil@example.com", formModel.Email);
            Assert.AreEqual("1234567890", formModel.PhoneNumber);
        }
    }
}
