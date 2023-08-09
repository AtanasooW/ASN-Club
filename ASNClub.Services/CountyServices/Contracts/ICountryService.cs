using ASNClub.ViewModels.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.CountyServices.Contracts
{
    public interface ICountryService
    {
        public Task<List<CountryViewModel>> GetCountryNamesAsync();
    }
}
