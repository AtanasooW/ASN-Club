using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.Models;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace ASNClub.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        public ShopController(IProductService _productService, ICategoryService _categoryService)
        {
            this.productService = _productService;
            this.categoryService = _categoryService;
        }
        public async Task<IActionResult> All([FromQuery] AllProductQueryModel queryModel)
        {
            AllProductsSortedModel serviceModel = await productService.GetAllProductsAsync(queryModel);
            queryModel.Products = serviceModel.Products;
            queryModel.TotalProducts = serviceModel.TotalProducts;
            queryModel.Categories = await categoryService.AllCategoriesNamesAsync();

            return this.View(queryModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ProductFormModel formModel = new ProductFormModel();
            return this.View(formModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(ProductFormModel formModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(formModel);
            }
            await productService.AddProductAsync(formModel);
            return RedirectToAction("All");
        }
    }
}
