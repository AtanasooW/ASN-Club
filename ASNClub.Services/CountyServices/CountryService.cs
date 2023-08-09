using ASNClub.Data;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.ViewModels.Country;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.CountyServices
{
    public class CountryService : ICountryService
    {
        readonly private ASNClubDbContext dbContext;
        public CountryService(ASNClubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<CountryViewModel>> GetCountryNamesAsync()
        {
            return await dbContext.Countries.Select(x => new CountryViewModel
            {
               Id = x.Id,
               Name = x.Name
            }).ToListAsync();
        }
    }
}
