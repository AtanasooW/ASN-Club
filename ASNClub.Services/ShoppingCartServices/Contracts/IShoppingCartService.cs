using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ShoppingCartServices.Contracts
{
    public interface IShoppingCartService
    {
        public Task AddProductToCartAsync(int id, int quantity, Guid userId);
        public Task<ShoppingCartViewModel> GetShoppingCartByUserIdAsync(Guid userId);
        public Task RemoveProductFromCartAsync(int id, int shoppingCartId, Guid userId);
        public Task<string> GetTotal(Guid userId);
        public Task UpdateProductQuantityAsync(int shoppingCartItemId, int newQuantity);
        public Task<ICollection<ProductAllViewModel>> GetAllProductsFromShoppingCartAsync(Guid userId);
    }
}
