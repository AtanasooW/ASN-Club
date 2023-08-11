using ASNClub.Infrastructure.Extensions;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Order;
using Microsoft.AspNetCore.Mvc;
using static ASNClub.Common.NotificationMessagesConstants;


namespace ASNClub.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IAddressService addressService;
        private readonly ICountryService countryService;
        private readonly IProductService productService;
        public OrderController(IOrderService _orderService, IAddressService _addressService, ICountryService _countryService, IProductService _productService)
        {
            orderService = _orderService;
            addressService = _addressService;
            countryService = _countryService;
            productService = _productService;
        }
        
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.GetId();
            var model = await orderService.GetMyOrdersByIdAsync(Guid.Parse(userId));
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            var userId = User.GetId();
            var model = await orderService.GetMyOrderDetailsByIdAsync(Guid.Parse(userId), Guid.Parse(id));
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.GetId();
            var model = await orderService.GetOrderByUserIdAsync(Guid.Parse(userId));
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel model)
        {

            try
            {
                await orderService.PlaceOrderAsync(model);
                TempData["SuccessMessage"] = "Successfully placed a order";
                return RedirectToAction("All", "Shop");
            }
            catch (Exception e )
            {

                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("All", "Shop");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CheckoutWitoutProfile(int id, int quantity)
        {
            var model = new OrderViewModel();
            var address = new AddressViewModel();
            var product = await productService.GetProductByIdAsync(id);
            model.ShippingAdress = address;
            model.Products.Add(product);
            model.ShippingAdress.Countries = await countryService.GetCountryNamesAsync();
            model.ProductId = id;
            model.Quantity = quantity;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CheckoutWitoutProfile(OrderViewModel model)
        {

            try
            {
                await orderService.PlaceOrderAsync(model);
                TempData["SuccessMessage"] = "Successfully placed a order";
                return RedirectToAction("All", "Shop");
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> SetDefaultAddress(Guid addressId, Guid userId)
        {
            // Update the IsDefault property in your service
            await addressService.SetDefaultAddressAsync(addressId, userId);

            return Json(new { success = true }); // Return success response
        }
    }
}
