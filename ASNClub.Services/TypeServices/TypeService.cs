using ASNClub.Data;
using ASNClub.Services.TypeServices.Contracts;
using ASNClub.ViewModels.Category;
using ASNClub.ViewModels.Color;
using ASNClub.ViewModels.Type;
using Microsoft.EntityFrameworkCore;

namespace ASNClub.Services.TypeServices
{
    public class TypeService : ITypeService
    {
        private readonly ASNClubDbContext dbContext;
        public TypeService(ASNClubDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public async Task<IEnumerable<string>> AllTypeNamesAsync()
        {
            IEnumerable<string> types = await dbContext.Types
                .AsNoTracking()
                .Select(x => x.Name)
                .ToListAsync();
            return types;
        }

        public async Task<IEnumerable<ProductTypeFormModel>> AllTypesAsync()
        {
            IEnumerable<ProductTypeFormModel> types = await dbContext.Types
               .AsNoTracking()
               .Select(x => new ProductTypeFormModel
               {
                   Id = x.Id,
                   Name = x.Name
               }).ToListAsync();
            return types;
        }
    }
}
