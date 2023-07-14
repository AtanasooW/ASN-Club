using ASNClub.Services.Models;
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
        public Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel);
        public Task<IEnumerable<string>> AllMakeNamesAsync();
        public Task<IEnumerable<string>> AllModelNamesAsync(string make);
    }
}
