using ASNClub.Services.Models;
using ASNClub.ViewModels.Product;
using ASNClub.Data.Models.Product;
using ASNClub.Data;
using ASNClub.Services.ProductServices.Contracts;
using Microsoft.EntityFrameworkCore;
using ASNClub.ViewModels.Product.Enums;

namespace ASNClub.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ASNClubDbContext dbContext;
        public ProductService(ASNClubDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public async Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel)
        {
            IQueryable<Product> products = dbContext.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                products = products.Where(x => x.Category.Name == queryModel.Category);
            }
            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                products = products
                    .Where(h => EF.Functions.Like(h.Make, wildCard) ||
                                EF.Functions.Like(h.Model, wildCard) ||
                                EF.Functions.Like(h.Type.Name, wildCard));
            }
            products = queryModel.ProductSorting switch
            {
                ProductSorting.PriceAscending => products.OrderBy(x => x.Price),
                ProductSorting.PriceDescending => products.OrderByDescending(x => x.Price)
            };
            return null;
            //IEnumerable<ProductAllViewModel> allProducts = await products
            //    .Skip((queryModel.CurrentPage - 1) * queryModel.ProductsPerPage)
            //    .Take(queryModel.ProductsPerPage)
            //    .Select(x => new ProductAllViewModel
            //    {
            //        Id = x.Id.ToString(),
            //    })
        }
    }
}