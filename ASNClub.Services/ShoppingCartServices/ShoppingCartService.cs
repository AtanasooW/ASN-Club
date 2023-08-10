using ASNClub.Data;
using ASNClub.Data.Models.Orders;
using ASNClub.Data.Models.Product;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.ViewModels.Discount;
using ASNClub.ViewModels.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ShoppingCartServices
{

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ASNClubDbContext dbContext;
        private readonly IProductService productService;
        public ShoppingCartService(ASNClubDbContext _dbContext, IProductService _productService)
        {
            dbContext = _dbContext;
            productService = _productService;
        }


        public async Task AddProductToCartAsync(int id, int quantity, Guid userId)
        {
            var shoppingCart = await dbContext.ShoppingCarts.Include(x => x.ShoppingCartItems).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (shoppingCart != null)
            {
                var shoppingCartItem = shoppingCart.ShoppingCartItems.Where(x => x.ProductId == id).FirstOrDefault();
                if (shoppingCartItem != null)
                {
                    shoppingCartItem.Quantity += quantity;
                }
                else
                {
                    var product = await productService.GetProductByIdAsync(id);
                    ShoppingCartItem newShoppingCartItem = new ShoppingCartItem()
                    {
                        ProductId = id,
                        Product = product,
                        Quantity = quantity,
                        ShoppingCartId = shoppingCart.Id
                    };
                    shoppingCart.ShoppingCartItems.Add(newShoppingCartItem);
                }
            }
            else
            {
                shoppingCart = new ShoppingCart()
                {
                    UserId = userId
                };
                await dbContext.ShoppingCarts.AddAsync(shoppingCart);
                ShoppingCartItem shoppingCartItem = new ShoppingCartItem()
                {
                    ProductId = id,
                    Quantity = quantity,
                    ShoppingCartId = shoppingCart.Id
                };
                shoppingCart.ShoppingCartItems.Add(shoppingCartItem);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task<ShoppingCartViewModel?> GetShoppingCartByUserIdAsync(Guid userId)
        {
            return await dbContext.ShoppingCarts.Include(x => x.ShoppingCartItems)
                .Where(x => x.UserId == userId)
                .Select(x => new ShoppingCartViewModel
                {
                    Id = x.Id,
                    UserId = userId,
                    ShoppingCartItems = x.ShoppingCartItems.Select(p => new ShoppingCartItemViewModel
                    {
                        Id = p.Id,
                        ShoppingCartId = p.ShoppingCartId,
                        ProductId = p.Product.Id,
                        Quantity = p.Quantity,
                        ProductQuantity = p.Product.Quantity,
                        Make = p.Product.Make,
                        Model = p.Product.Model,
                        Price = p.Product.Price,
                        Material = p.Product.Material.Name,
                        Type = p.Product.Type.Name,
                        Color = p.Product.Color.Name,
                        ImgUrl = p.Product.ImgUrls.FirstOrDefault(x=> x.ProductId == p.Product.Id).ImgUrl.Url,
                        Discount = new ProductDiscountFormModel
                        {
                            IsDiscount = p.Product.Discount.IsDiscount,
                            DiscountRate = p.Product.Discount.DiscountRate,
                            StartDate = p.Product.Discount.StartDate,
                            EndDate = p.Product.Discount.EndDate
                        },
                    }).ToList()
                }).FirstOrDefaultAsync();
        }


        public async Task<string> GetTotal(Guid userId)
        {
            var shoppingCart = await dbContext.ShoppingCarts
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x=> x.Discount)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
            if (shoppingCart != null)
            {
                decimal total = 0;
                foreach (var item in shoppingCart.ShoppingCartItems)
                {
                    if (item.Product.Discount.IsDiscount)
                    {
                        total += (item.Product.Price - ((item.Product.Price * (decimal)item.Product.Discount.DiscountRate) / 100)) * item.Quantity;
                    }
                    else
                    {
                        total += item.Product.Price * item.Quantity;
                    }
                }
                return total.ToString("F2");
            }
            else
            {
                throw new InvalidOperationException("Invalid shopping cart");
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveProductFromCartAsync(int id, int shoppingCartId, Guid userId)
        {
            var shoppingCart = await dbContext.ShoppingCarts.Include(x => x.ShoppingCartItems).Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (shoppingCart != null)
            {
                var shoppingCartItem = shoppingCart.ShoppingCartItems.Where(x => x.Id == id).FirstOrDefault();
                if (shoppingCartItem != null)
                {
                    shoppingCart.ShoppingCartItems.Remove(shoppingCartItem);
                    dbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
                else
                {
                throw new InvalidOperationException("Invalid product");

                   
                }
            }
            else
            {
                throw new InvalidOperationException("Invalid shopping cart");
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductQuantityAsync(int shoppingCartItemId, int newQuantity)
        {
            var shoppingCartItem = await dbContext.ShoppingCartItems.FindAsync(shoppingCartItemId);
            if (shoppingCartItem != null)
            {
                shoppingCartItem.Quantity = newQuantity;
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
