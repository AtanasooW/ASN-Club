using ASNClub.Infrastructure.Extensions;
using ASNClub.Services.ShoppingCartServices;
using ASNClub.Services.ShoppingCartServices.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ASNClub.Common.NotificationMessagesConstants;
namespace ASNClub.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        readonly private IShoppingCartService shoppingCartService;
        public ShoppingCartController(IShoppingCartService _shoppingCartService)
        {
            shoppingCartService = _shoppingCartService;            
        }
        public async Task<IActionResult> MyShoppingCart()
        {
            var userId = User.GetId();
            var model = await shoppingCartService.GetShoppingCartByUserIdAsync(Guid.Parse(userId));
            return View(model);
        }
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var userId = User.GetId();
            await shoppingCartService.AddProductToCartAsync(id, quantity, Guid.Parse(userId));
            TempData[SuccessMessage] = "Successfuly added item to the shopping cart";

            return RedirectToAction("Details", "Shop", new { id = id});
        }
        public async Task<IActionResult> RemoveItemFromCart(int id, int shoppingCartId)
        {
            var userId = User.GetId();
            await shoppingCartService.RemoveProductFromCartAsync(id, shoppingCartId, Guid.Parse(userId));
            TempData[SuccessMessage] = "Successfuly remove item from the shopping cart";

            return RedirectToAction("MyShoppingCart");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            await shoppingCartService.UpdateProductQuantityAsync(itemId, quantity);
            return Json(new { success = true });
        }
    }
}
