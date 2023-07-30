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
    public class MaterialService : IMaterialService
    {
        private readonly ASNClubDbContext dbContext;
        public MaterialService(ASNClubDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public async Task<IEnumerable<ProductMaterialFormModel>> AllCategoriesAsync()
        {
            IEnumerable<ProductMaterialFormModel> categories = await dbContext.Materials
                .AsNoTracking()
                .Select(x => new ProductMaterialFormModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return categories;
        }

        public async Task<IEnumerable<string>> AllCategoryNamesAsync()
        {
            IEnumerable<string> categories = await dbContext.Materials
             .AsNoTracking()
             .Select(x => x.Name)
             .ToListAsync();
            return categories;
        }
    }
}
