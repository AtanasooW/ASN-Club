using ASNClub.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.CategoryServices.Contracts
{
    public interface IMaterialService
    {
        public Task<IEnumerable<ProductMaterialFormModel>> AllCategoriesAsync();
        public Task<IEnumerable<string>> AllCategoryNamesAsync();
    }
}
