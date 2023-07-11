using ASNClub.Services.Models;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;

namespace ASNClub.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService productService;
        public ShopController(IProductService _productService)
        {
            this.productService = _productService;
        }
        public async IActionResult All([FromQuery] AllProductQueryModel queryModel)
        {
            AllProductsSortedModel serviceModel = await productService.GetAllProductsAsync(queryModel);
        }
    }
}
