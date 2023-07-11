using ASNClub.Data;
using ASNClub.Services.CategoryServices.Contracts;
using ASNClub.ViewModels.Category;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<ProductCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<ProductCategoryFormModel> categories = await dbContext.Categories
                .AsNoTracking()
                .Select(x => new ProductCategoryFormModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<string>> AllCategoriesNamesAsync()
        {
            IEnumerable<string> categories = await dbContext.Categories
             .AsNoTracking()
             .Select(x => x.Name)
             .ToListAsync();
            return categories;
        }
    }
}
