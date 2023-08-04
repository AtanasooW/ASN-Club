using ASNClub.Data.Models;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.ColorServices;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.TypeServices;
using ASNClub.Services.TypeServices.Contracts;
using ASNClub.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public AdminController(RoleManager<IdentityRole<Guid>> _roleManager, UserManager<ApplicationUser> _userManager, IProductService _productService, IMaterialService _categoryService, IColorService _colorService, ITypeService _typeService)
        {
            this.roleManager = _roleManager;
            this.userManager = _userManager;
            this.productService = _productService;
            this.categoryService = _categoryService;
            this.colorService = _colorService;
            this.typeService = _typeService;
        }
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var productFormModel = await productService.GetProductForEditByIdAsync(id);
            productFormModel.Materials = await categoryService.AllCategoriesAsync();
            productFormModel.Colors = await colorService.AllColorsAsync();
            productFormModel.Types = await typeService.AllTypesAsync();
            return View(productFormModel);
        }
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("All", "Shop");
        }

    }
}
