﻿using ASNClub.Data.Models.Product;
using ASNClub.Services.Models;
using ASNClub.ViewModels.Color;
using ASNClub.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ProductServices.Contracts
{
    public interface IProductService
    {
        public Task AddProductAsync(ProductFormModel formModel);
        public Task EditProductAsync(ProductFormModel formModel);
        public Task DeleteProductByIdAsync(int id);
        public Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel);
        public Task<ProductDetailsViewModel> GetProductDetailsByIdAsync(int id);
        public Task<ProductFormModel> GetProductForEditByIdAsync(int id);
        public Task<ProductAllViewModel> GetProductByIdAsync(int id);
        public Task<Product> GetProductOfTypeProductByIdAsync(int id);

        public Task AddRatingAsync(int id, int ratingValue, string? userId);
        public Task AddCommentAsync(int id, string username, string ownerId, string content);
        public Task RemoveCommentAsync(Guid id, int productId);

        public Task<IEnumerable<string>> AllMakeNamesAsync();
        public Task<IEnumerable<string>> AllModelNamesAsync(string make);
        public Task<List<ColorProductIdViewModel>> GetAllColorsForProductAsync(string make, string model, int typeId);
    }
}
