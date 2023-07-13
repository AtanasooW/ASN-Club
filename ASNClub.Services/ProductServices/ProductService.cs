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

        public async Task AddProductAsync(ProductFormModel formModel)
        {
            Product product = new Product()
            {
                Make = formModel.Make,
                Model = formModel.Model,
                Price = formModel.Price,
                Description = formModel.Description,
                TypeId = formModel.TypeId,
                ColorId = formModel.ColorId == 1 ? null : formModel.ColorId,
                CategoryId = formModel.CategoryId
            };

            //Cheking if the discount exits. If the discount exists we place it on the product. Otherwise we make a new discount.
            Discount? isExist = await dbContext.Discounts.FirstOrDefaultAsync(x=> x.IsDiscount == true &&
            x.DiscountRate == formModel.Discount.DiscountRate &&
            x.StartDate == formModel.Discount.StartDate &&
            x.EndDate == formModel.Discount.EndDate);

            if (isExist != null)
            {
                product.DiscountId = isExist.Id;
            }
            else
            {
                Discount discount = new Discount();
                discount.IsDiscount = formModel.Discount.IsDiscount;

                if (formModel.Discount.IsDiscount)
                {
                    discount.DiscountRate = formModel.Discount.DiscountRate;
                    discount.StartDate = formModel.Discount.StartDate;
                    discount.EndDate = formModel.Discount.EndDate;
                }
                await dbContext.Discounts.AddAsync(discount);
                await dbContext.SaveChangesAsync();

                product.DiscountId = discount.Id;
            }
          
            ImgUrl imgUrl = new ImgUrl()
            {
                Url = formModel.ImgUrl
            };
            await dbContext.Products.AddAsync(product);
            await dbContext.ImgUrls.AddAsync(imgUrl);
            await dbContext.SaveChangesAsync();

            ProductImgUrl productImg = new ProductImgUrl()
            {
                ProductId = product.Id,
                ImgUrlId = imgUrl.Id
            };
            await dbContext.ProductsImgUrls.AddAsync(productImg);
            product.ImgUrls.Add(productImg);

            await dbContext.SaveChangesAsync();
        }

        public async Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel)
        {
            IQueryable<Product> products = dbContext.Products.AsQueryable().AsNoTracking();

            //TO DO The logic for model drop down and make
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

            IEnumerable<ProductAllViewModel> allProducts = await products
                .AsNoTracking()
                .Skip((queryModel.CurrentPage - 1) * queryModel.ProductsPerPage)
                .Take(queryModel.ProductsPerPage)
                .Select(p => new ProductAllViewModel
                {
                    Id = p.Id.ToString(),
                    Make = p.Make,
                    Model = p.Model,
                    Price = p.Price,
                    ImgUrl = p.ImgUrls.FirstOrDefault(x => x.ProductId == p.Id).ImgUrl.Url,
                    Type = p.Type.Name,
                    Color = p.Color.Name,
                    IsDiscount = p.Discount.IsDiscount

                }).ToListAsync();
            AllProductsSortedModel sortedModel = new AllProductsSortedModel()
            {
                Products = allProducts,
                TotalProducts = products.Count()
            };
            return sortedModel;
        }
    }
}