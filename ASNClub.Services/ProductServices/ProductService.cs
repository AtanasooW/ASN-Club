﻿using ASNClub.Services.Models;
using ASNClub.ViewModels.Product;
using ASNClub.Data.Models.Product;
using ASNClub.Data;
using ASNClub.Services.ProductServices.Contracts;
using Microsoft.EntityFrameworkCore;
using ASNClub.ViewModels.Product.Enums;
using ASNClub.ViewModels.Discount;

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
                Quantity = formModel.Quantity,
                ColorId = formModel.ColorId == 1 ? null : formModel.ColorId,
                MaterialId = formModel.CategoryId
            };

            //Cheking if the discount exits. If the discount exists we place it on the product. Otherwise we make a new discount.
            Discount? isExist = await dbContext.Discounts.FirstOrDefaultAsync(x => x.IsDiscount == formModel.Discount.IsDiscount &&
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
            ICollection<ImgUrl> images = new HashSet<ImgUrl>();
            foreach (var item in formModel.ImgUrls)
            {
                ImgUrl imgUrl = new ImgUrl()
                {
                    Url = item
                };
                images.Add(imgUrl);
                await dbContext.ImgUrls.AddAsync(imgUrl);

            }

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();

            foreach (var item in images)
            {
                ProductImgUrl productImg = new ProductImgUrl()
                {
                    ProductId = product.Id,
                    ImgUrlId = item.Id
                };
                await dbContext.ProductsImgUrls.AddAsync(productImg);
                product.ImgUrls.Add(productImg);

            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel)
        {
            IQueryable<Product> products = dbContext.Products.AsQueryable().AsNoTracking();

            //TO DO The logic for model drop down and make
            if (!string.IsNullOrWhiteSpace(queryModel.Material))
            {
                products = products.Where(x => x.Material.Name == queryModel.Material);
            }
            if (!string.IsNullOrWhiteSpace(queryModel.Type))
            {
                products = products.Where(x => x.Type.Name == queryModel.Type);
            }
            if (!string.IsNullOrWhiteSpace(queryModel.Make))
            {
                products = products.Where(x => x.Make == queryModel.Make);
            }
            //fixing bug when model is chosen and you press all in the make drop down menu
            if (string.IsNullOrWhiteSpace(queryModel.Make) && !string.IsNullOrWhiteSpace(queryModel.Model))
            {
                queryModel.Model = null;
            }
            if (!string.IsNullOrWhiteSpace(queryModel.Model))
            {
                products = products.Where(x => x.Model == queryModel.Model);
            }
            if (!string.IsNullOrWhiteSpace(queryModel.SearchString)) //TO DO Search bar is worikng with only one word fix that 
            {
                string[] searchStrings = queryModel.SearchString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string wildCardForMake = $"%{searchStrings[0].ToLower()}%";
                string wildCardForModel = $"%{searchStrings[1].ToLower()}%";

                products = products
                    .Where(h => EF.Functions.Like(h.Make.ToLower(), wildCardForMake) &&
                                EF.Functions.Like(h.Model.ToLower(), wildCardForModel));
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

        public async Task<IEnumerable<string>> AllMakeNamesAsync()
        {
            IEnumerable<string> makes = await dbContext.Products
               .AsNoTracking()
               .Select(x => x.Make)
               .Distinct()
               .ToListAsync();
            return makes;
        }
        public async Task<IEnumerable<string>> AllModelNamesAsync(string make)
        {
            IEnumerable<string> models = await dbContext.Products
               .AsNoTracking()
               .Where(x => x.Make == make)
               .Select(x => x.Model)
               .Distinct()
               .ToListAsync();
            return models;
        }

        public async Task<ProductDetailsViewModel?> GetProductDetails(int id)
        {
            return await dbContext.Products
                .Where(x => x.Id == id)
                .Include(x => x.Ratings) // Include the Ratings collection
                .Select(x => new ProductDetailsViewModel
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    Price = x.Price,
                    Description = x.Description,
                    Material = x.Material.Name,
                    Type = x.Type.Name,
                    Rating = x.Ratings.Count() == 0 ? 0.0 : x.Ratings.Average(r => r.RatingValue),
                    ImgUrls = x.ImgUrls.Select(i => i.ImgUrl.Url).ToList(), // Convert to List<string>
                    Quantity = x.Quantity,
                    Color = x.Color.Name, // Use null-conditional operator in case Color is null
                    Discount = new ProductDiscountFormModel
                    {
                        IsDiscount = x.Discount.IsDiscount,
                        DiscountRate = x.Discount.DiscountRate,
                        StartDate = x.Discount.StartDate,
                        EndDate = x.Discount.EndDate
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task AddRatingAsync(int id, int ratingValue, string? userId)
        {
            var rating = new Rating()
            {
                ProductId = id,
                UserId = Guid.Parse(userId),
                RatingValue = ratingValue
            };
            var product = await GetProductByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException("Product doesn't exist");
            }
            await dbContext.Ratings.AddAsync(rating);
            product.Ratings.Add(rating);
            await dbContext.SaveChangesAsync();

        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await dbContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}