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
        public Task<AllProductsSortedModel> GetAllProductsAsync(AllProductQueryModel queryModel);
    }
}
