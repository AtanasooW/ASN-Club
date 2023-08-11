using ASNClub.Infrastructure.Extensions;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;

namespace ASNClub.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IAddressService addressService;
        public OrderController(IOrderService _orderService, IAddressService _addressService)
        {
                orderService = _orderService;
            addressService = _addressService;
        }
        public async Task<IActionResult> Checkout()
        {
            var userId = User.GetId();
            var model = await orderService.GetOrderByUserIdAsync(Guid.Parse(userId));
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId,Guid userId)
        {
            // Update the IsDefault property in your service
            await addressService.SetDefaultAddressAsync(addressId, userId);

            return Json(new { success = true }); // Return success response
        }
        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderViewModel model)
        {
            try
            {
                await orderService.PlaceOrderAsync(model);
                return RedirectToAction("All","Shop");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
