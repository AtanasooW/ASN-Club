using ASNClub.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.CategoryServices.Contracts
{
    public interface ICategoryService
    {
        public Task<IEnumerable<ProductCategoryFormModel>> AllCategoriesAsync();
    }
}
