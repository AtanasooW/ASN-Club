using ASNClub.Data.Models.Product;
using ASNClub.Hubs;
using ASNClub.Infrastructure.Extensions;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.Services.Models;
using ASNClub.Services.ProductServices.Contracts;
using ASNClub.Services.TypeServices.Contracts;
using ASNClub.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using static ASNClub.Common.NotificationMessagesConstants;


namespace ASNClub.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductService productService;
        private readonly IMaterialService categoryService;
        private readonly IColorService colorService;
        private readonly ITypeService typeService;
        //private readonly IHubContext<CommentsHub> commentsHubContext;

        public ShopController(IHubContext<CommentsHub> _commentsHubContext,IProductService _productService, IMaterialService _categoryService, IColorService _colorService, ITypeService _typeService)
        {
            this.productService = _productService;
            this.categoryService = _categoryService;
            this.colorService = _colorService;
            this.typeService = _typeService;
            //commentsHubContext = _commentsHubContext;

        }
        public async Task<IActionResult> All([FromQuery] AllProductQueryModel queryModel)
        {
            AllProductsSortedModel serviceModel = await productService.GetAllProductsAsync(queryModel);
            queryModel.Products = serviceModel.Products;
            queryModel.TotalProducts = serviceModel.TotalProducts;
            queryModel.Categories = await categoryService.AllCategoryNamesAsync();
            queryModel.Types = await typeService.AllTypeNamesAsync();
            queryModel.Makes = await productService.AllMakeNamesAsync();
            if (queryModel.Make != null)
            {
                queryModel.Models = await productService.AllModelNamesAsync(queryModel.Make);
            }
            return this.View(queryModel);
        }
        public async Task<IActionResult> Details (int id)
        {
            var model = await productService.GetProductDetailsByIdAsync(id);
            return this.View(model);
        }
        public async Task<IActionResult> AddRating(int id, int ratingValue)
        {
            var userId = User.GetId();
            try
            {
                await productService.AddRatingAsync(id,ratingValue, userId);
                TempData["SuccessMessage"] = "You successfully rated a product";
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                throw;
            }
            return RedirectToAction("Details", new{ id = id});
        }
        public async Task<IActionResult> AddComment(int id, string username, string ownerId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                // You may want to handle the case where the content is null or empty.
                // For example, you could return an error message or perform some action.
                TempData["ErrorMesage"] = "Comment content cannot be null or empty.";
                return RedirectToAction("Details", new { id = id });
            }
            if (content != "COMMENT_VALUE")
            {
               await productService.AddCommentAsync(id,username,ownerId,content);
                TempData["SuccessMessage"] = "You successfully added a comment to a product";
            }
            return RedirectToAction("Details", new {id = id });
        }
        
    }
}
