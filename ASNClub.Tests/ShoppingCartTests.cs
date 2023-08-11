using ASNClub.Data;
using ASNClub.Data.Models.Orders;
using ASNClub.Services.ProductServices;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.ShoppingCartServices;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.ViewModels.Discount;
using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASNClub.Tests
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private DbContextOptions<ASNClubDbContext> dbOptions;
        private ASNClubDbContext dbContext;

        private IShoppingCartService shoppingCartService;
        private IProductService productService;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<ASNClubDbContext>()
                .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new ASNClubDbContext(this.dbOptions);

            this.dbContext.Database.EnsureCreated();
            DatabaseSeeder.SeedDatabaseAsync(this.dbContext, null);
            this.productService = new ProductService(this.dbContext);
            this.shoppingCartService = new ShoppingCartService(this.dbContext, productService);
        }

        [Test]
        public async Task AddProductToCartAsync_Should_Add_Product_To_Cart()
        {
            // Arrange

            var productId = 1;
            var userId = Guid.NewGuid();

            // Act
            await shoppingCartService.AddProductToCartAsync(productId, 2, userId);

            // Assert
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            Assert.NotNull(shoppingCart);
            Assert.AreEqual(1, shoppingCart.ShoppingCartItems.Count);
        }

        [Test]
        public async Task GetAllProductsFromShoppingCartAsync_Should_Return_All_Products_In_Shopping_Cart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var productsInCart = await shoppingCartService.GetAllProductsFromShoppingCartAsync(userId);

            // Assert
            Assert.NotNull(productsInCart);
            Assert.IsInstanceOf<ICollection<ProductAllViewModel>>(productsInCart);
        }

        [Test]
        public async Task GetShoppingCartByUserIdAsync_Should_Return_Shopping_Cart_By_User_Id()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var shoppingCart = await shoppingCartService.GetShoppingCartByUserIdAsync(userId);

            // Assert
            Assert.NotNull(shoppingCart);
            Assert.IsInstanceOf<ShoppingCartViewModel>(shoppingCart);
        }

        [Test]
        public async Task GetTotal_Should_Calculate_Total_Amount_In_Shopping_Cart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var total = await shoppingCartService.GetTotal(userId);

            // Assert
            Assert.NotNull(total);
            Assert.IsInstanceOf<string>(total);
        }

        [Test]
        public async Task RemoveProductFromCartAsync_Should_Remove_Product_From_Cart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Add a product to the cart
            await shoppingCartService.AddProductToCartAsync(1, 1, userId);

            // Act

            var shoppingCart = await dbContext.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var shoppingCartItem = shoppingCart.ShoppingCartItems.FirstOrDefault();

            await shoppingCartService.RemoveProductFromCartAsync(shoppingCartItem.Id, userId);

            // Assert
            Assert.AreEqual(0, shoppingCart.ShoppingCartItems.Count);

        }

        [Test]
        public async Task UpdateProductQuantityAsync_Should_Update_Product_Quantity_In_Cart()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Add a product to the cart
            await shoppingCartService.AddProductToCartAsync(1, 1, userId);

            // Act

            var shoppingCart = await dbContext.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var shoppingCartItem = shoppingCart.ShoppingCartItems.FirstOrDefault();
            await shoppingCartService.UpdateProductQuantityAsync(shoppingCartItem.Id, 3);

            // Assert
            Assert.AreEqual(3, shoppingCartItem.Quantity);
        }
    }
}
