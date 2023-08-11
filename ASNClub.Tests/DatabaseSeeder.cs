using ASNClub.Data;
using ASNClub.Data.Models;
using ASNClub.Data.Models.AddressModels;
using ASNClub.Data.Models.Orders;
using ASNClub.Data.Models.Product;
using ASNClub.ViewModels.Discount;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Tests
{
    public class DatabaseSeeder
    {
        public static async void SeedDatabaseAsync(ASNClubDbContext dbContext, IServiceProvider serviceProvider)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = new ApplicationUser
            {
                UserName = "exampleuser",
                Email = "example@example.com",
                FirstName = "John",
                SurnameName = "Doe"
            };

            await userManager.CreateAsync(user, "Password123"); // Replace with the desired password

            await dbContext.SaveChangesAsync();

            var shippingAddress = new Address
            {
                Id = Guid.NewGuid(),
                CountryId = 1,
                City = "Sofia",
                Street1 = "Tzar Boris 3",
                StreetNumber = "57",
                PostalCode = "1000",
                IsDefault = true,

            };
            var shoppingCart = new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,

            };
            Discount discount = new Discount()
            {
                Id = 1,
                IsDiscount = false
            };
            await dbContext.Discounts.AddAsync(discount);
            Product product = new Product()
            {
                Id = 1,
                Make = "Browning",
                Model = "Mk3",
                TypeId = 1,
                Price = 320,
                DiscountId = 1,
                Description = "The best grips on the market",
                Quantity = 5,
                MaterialId = 3,
                ColorId = 1,
            };
            await dbContext.Products.AddAsync(product);

            ImgUrl imgUrl = new ImgUrl()
            {
                Url = "http://www.gunsgripasn.com/images/chireni/Chireni_sportna_strelba_MAGWELL.png"
            };
            await dbContext.ImgUrls.AddAsync(imgUrl);
            await dbContext.SaveChangesAsync();
            ProductImgUrl productImg = new ProductImgUrl()
            {
                ProductId = product.Id,
                ImgUrlId = imgUrl.Id
            };
            await dbContext.ProductsImgUrls.AddAsync(productImg);
            product.ImgUrls.Add(productImg);
            await dbContext.SaveChangesAsync();

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OrderDate = DateTime.Now,
                ShippingAdressId = shippingAddress.Id,
                OrderTotal = 100.00m, 
                OrderStatusId = 1,
                ShoppingCartId = shoppingCart.Id
            };

            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = 2 
            };
            dbContext.Users.Add(user);
            dbContext.Addresses.Add(shippingAddress);
            dbContext.Products.Add(product);
            dbContext.ShoppingCarts.Add(shoppingCart);
            dbContext.Orders.Add(order);
            dbContext.OrdersItems.Add(orderItem);

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }
    }
}
