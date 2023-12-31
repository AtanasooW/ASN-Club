﻿using ASNClub.Services.Models;
using ASNClub.ViewModels.Product;
using ASNClub.Data.Models.Product;
using ASNClub.Data;
using ASNClub.Services.ProductServices.Contracts;
using Microsoft.EntityFrameworkCore;
using ASNClub.ViewModels.Product.Enums;
using ASNClub.ViewModels.Discount;
using ASNClub.ViewModels.Comment;
using ASNClub.Services.ColorServices;
using ASNClub.Services.TypeServices;
using System.Linq;
using ASNClub.ViewModels.Color;

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
                MaterialId = formModel.MaterialId
            };

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
        public async Task EditProductAsync(ProductFormModel formModel)
        {
            if (formModel.ColorId == 1)
            {
                formModel.ColorId = null;
            }
            var product = await GetProductOfTypeProductByIdAsync((int)formModel.Id);

            if (formModel.Discount.IsDiscount)
            {
                product.Discount.IsDiscount = formModel.Discount.IsDiscount;
                product.Discount.DiscountRate = formModel.Discount.DiscountRate;
                product.Discount.StartDate = formModel.Discount.StartDate;
                product.Discount.EndDate = formModel.Discount.EndDate;
            }
            else
            {
                product.Discount.IsDiscount = false;
                product.Discount.DiscountRate = null;
                product.Discount.StartDate = null;
                product.Discount.EndDate = null;
            }

            product.Make = formModel.Make;
            product.Model = formModel.Model;
            product.Price = formModel.Price;
            product.Description = formModel.Description;
            product.Quantity = formModel.Quantity;
            product.TypeId = formModel.TypeId;
            product.MaterialId = formModel.MaterialId;
            product.ColorId = formModel.ColorId;

            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteProductByIdAsync(int id)
        {
            var product = await GetProductOfTypeProductByIdAsync(id);
            dbContext.Products.Remove(product);
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
                ProductSorting.PriceDescending => products.OrderByDescending(x => x.Price),
                ProductSorting.RatingAscending => products.OrderBy(x => x.Ratings.Average(x => x.RatingValue)),
                ProductSorting.RatingDescending => products.OrderByDescending(x => x.Ratings.Average(x => x.RatingValue)),
                ProductSorting.Onsale => products.Where(x => x.Discount.IsDiscount)
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
                    IsDiscount = p.Discount.IsDiscount,
                    DiscountRate = p.Discount.DiscountRate,
                    Rating = p.Ratings.Count() >= 1 ? p.Ratings.Average(x => x.RatingValue) : 0

                }).ToListAsync();
            AllProductsSortedModel sortedModel = new AllProductsSortedModel()
            {
                Products = allProducts,
                TotalProducts = products.Count()
            };
            return sortedModel;
        }
        public async Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(int id)
        {
            var product = await dbContext.Products
                .Where(x => x.Id == id)
                .Include(x => x.Ratings)// Include the Ratings collection
                .Include(x => x.Discount)
                .Select(x => new ProductDetailsViewModel
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    Price = x.Price,
                    Description = x.Description,
                    Material = x.Material.Name,
                    Type = x.Type.Name,
                    TypeId = x.TypeId,
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
                    },
                    Comments = x.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        PostedOn = c.PostedOn,
                        ProductId = c.ProductId,
                        OwnerId = c.OwnerId,
                        OwnerName = c.OwnerName,
                        Text = c.Text
                    }).OrderByDescending(x => x.PostedOn).ToList()
                }).FirstOrDefaultAsync();
            if (product.Color == null || product.Color == "None")
            {
            return product;
            }
            var colors = await GetAllColorsForProductAsync(product.Make,product.Model, (int)product.TypeId);
            product.Colors = colors;
            return product;
        }
        public async Task<ProductFormModel> GetProductForEditByIdAsync(int id)
        {
            var product = await GetProductOfTypeProductByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException("Invalid product");
            }
            ProductDiscountFormModel discountFormModel = new ProductDiscountFormModel();

            if (product.Discount == null)
            {
                discountFormModel.IsDiscount = false;

            }
            else
            {
                discountFormModel.IsDiscount = product.Discount.IsDiscount;
                discountFormModel.DiscountRate = product.Discount.DiscountRate;
                discountFormModel.StartDate = product.Discount.StartDate;
                discountFormModel.EndDate = product.Discount.EndDate;
            }

            ProductFormModel formModel = new ProductFormModel()
            {
                Id = product.Id,
                Make = product.Make,
                Model = product.Model,
                Price = product.Price,
                Quantity = product.Quantity,
                MaterialId = product.MaterialId,
                TypeId = product.TypeId,
                ColorId = product.ColorId,
                Description = product.Description,
                Discount = discountFormModel,
                ImgUrls = await dbContext.ProductsImgUrls.Where(x => x.ProductId == product.Id).Select(x => x.ImgUrl.Url).ToListAsync()
            };
            return formModel;
        }
        public async Task<ProductAllViewModel?> GetProductByIdAsync(int id)
        {
            return await dbContext.Products.Where(x=> x.Id == id)
                .AsNoTracking()
                .Select(p => new ProductAllViewModel
                {
                    Id = p.Id.ToString(),
                    Make = p.Make,
                    Model = p.Model,
                    Price = p.Price,
                    ImgUrl = p.ImgUrls.FirstOrDefault(x => x.ProductId == p.Id).ImgUrl.Url,
                    Type = p.Type.Name,
                    Color = p.Color.Name,
                    IsDiscount = p.Discount.IsDiscount,
                    DiscountRate = p.Discount.DiscountRate,
                }).FirstOrDefaultAsync();
        }
        public async Task<Product> GetProductOfTypeProductByIdAsync(int id)
        {
            return await dbContext.Products.Include(x => x.Discount).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task AddRatingAsync(int id, int ratingValue, string? userId)
        {
            if (dbContext.Ratings.Any(x => x.UserId == Guid.Parse(userId) && x.ProductId == id))
            {
                throw new InvalidOperationException("Already rated");
            }
            var rating = new Rating()
            {
                ProductId = id,
                UserId = Guid.Parse(userId),
                RatingValue = ratingValue
            };
            var product = await GetProductOfTypeProductByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException("Product doesn't exist");
            }
            await dbContext.Ratings.AddAsync(rating);
            product.Ratings.Add(rating);
            await dbContext.SaveChangesAsync();

        }
        public async Task AddCommentAsync(int id, string username, string ownerId, string content)
        {
            var comment = new Comment()
            {
                Text = content,
                PostedOn = DateTime.Now,
                OwnerId = Guid.Parse(ownerId),
                OwnerName = username
            };
            await dbContext.Comments.AddAsync(comment);
            var product = await dbContext.Products.Where(x => x.Id == id).FirstOrDefaultAsync();
            product.Comments.Add(comment);
            await dbContext.SaveChangesAsync();
        }
        public async Task RemoveCommentAsync(Guid id, int productId)
        {
            var comment = await dbContext.Comments.Where(x => x.Id == id && x.ProductId == productId).FirstOrDefaultAsync();
            dbContext.Comments.Remove(comment);
            await dbContext.SaveChangesAsync();
        }








        public async Task<List<ColorProductIdViewModel>> GetAllColorsForProductAsync(string make, string model, int typeId)
        {
            return await dbContext.Products.Where(x => x.Make == make && x.Model == model && x.TypeId == typeId)
                .Select(x => new ColorProductIdViewModel
                {
                   ColorName = x.Color.Name,
                   ProductId = x.Id
               }).ToListAsync();
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

    }
}