using ASNClub.Data.Models;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.ColorServices;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.TypeServices;
using ASNClub.Services.TypeServices.Contracts;
using ASNClub.ViewModels.Order;
using ASNClub.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static ASNClub.Common.NotificationMessagesConstants;


namespace ASNClub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private RoleManager<IdentityRole<Guid>> roleManager;
        private UserManager<ApplicationUser> userManager;
        private readonly IProductService productService;
        private readonly IMaterialService categoryService;
        private readonly IColorService colorService;
        private readonly ITypeService typeService;
        private readonly IOrderService orderService;
        public AdminController(RoleManager<IdentityRole<Guid>> _roleManager,
            UserManager<ApplicationUser> _userManager,
            IProductService _productService,
            IMaterialService _categoryService,
            IColorService _colorService,
            ITypeService _typeService,
            IOrderService _orderService)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
            this.productService = _productService;
            this.categoryService = _categoryService;
            this.colorService = _colorService;
            this.typeService = _typeService;
            this.orderService = _orderService;
        }
     
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                return BadRequest("This role is already created");
            }
            var role = new IdentityRole<Guid>(roleName);
            var result = await roleManager.CreateAsync(role);
            return Ok(result);
        }
        public async Task<IActionResult> AllOrders()
        {
            var model = await orderService.GetAllOrdersAsync();
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            var model = await orderService.GetOrderDetailByIdAsync(Guid.Parse(id));
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditStatus(string id)
        {
            var model = await orderService.GetOrderStatusAsync(Guid.Parse(id));
            return View(model);
        }  
        [HttpPost]
        public async Task<IActionResult> EditStatus(OrderStatusViewModel model)
        {
            await orderService.EditOrderStatusAsync(model);
            return RedirectToAction("Details", new { id = model.OrderId });
        }
       
        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            var result = await userManager.AddToRoleAsync(user, "Admin");

            return Ok(result);
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await productService.DeleteProductByIdAsync(id);
            TempData["SuccessMessage"] = "Successfully delete product";
            return RedirectToAction("All", "Shop");
        }
    
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var productFormModel = await productService.GetProductForEditByIdAsync(id);
            productFormModel.Materials = await categoryService.AllCategoriesAsync();
            productFormModel.Colors = await colorService.AllColorsAsync();
            productFormModel.Types = await typeService.AllTypesAsync();
            return View(productFormModel);
        }
     
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                formModel.Materials = await categoryService.AllCategoriesAsync();
                formModel.Colors = await colorService.AllColorsAsync();
                formModel.Types = await typeService.AllTypesAsync();
                return this.View(formModel);
            }
            await productService.EditProductAsync(formModel);
            TempData["SuccessMessage"] = "Successfully edit product";
            return RedirectToAction("All", "Shop");
        }
       
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ProductFormModel formModel = new ProductFormModel();
            formModel.Materials = await categoryService.AllCategoriesAsync();
            formModel.Colors = await colorService.AllColorsAsync();
            formModel.Types = await typeService.AllTypesAsync();
            return this.View(formModel);
        }
       
        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                formModel.Materials = await categoryService.AllCategoriesAsync();
                formModel.Colors = await colorService.AllColorsAsync();
                formModel.Types = await typeService.AllTypesAsync();
                return this.View(formModel);
            }
            await productService.AddProductAsync(formModel);
            TempData["SuccessMessage"] = "Successfully added product";
            return RedirectToAction("All", "Shop");
        }
    }
}
