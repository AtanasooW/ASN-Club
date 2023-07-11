using ASNClub.Data;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly ASNClubDbContext dbContext;
        public CategoryService(ASNClubDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public Task<IEnumerable<ProductCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<ProductCategoryFormModel> categories = 
        }
    }
}
