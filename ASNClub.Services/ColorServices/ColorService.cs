using ASNClub.Data;
using ASNClub.Services.ColorServices.Contracts;
using ASNClub.ViewModels.Category;
using ASNClub.ViewModels.Color;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ColorServices
{
    public class ColorService : IColorService
    {
        private readonly ASNClubDbContext dbContext;
        public ColorService(ASNClubDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public async Task<IEnumerable<ProductColorFormModel>> AllColorsAsync()
        {
            IEnumerable<ProductColorFormModel> colors = await dbContext.Colors
                .AsNoTracking()
                .Select(x => new ProductColorFormModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
            return colors;
        }
    }
}
