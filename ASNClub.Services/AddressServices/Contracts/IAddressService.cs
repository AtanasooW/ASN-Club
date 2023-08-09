using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.AddressServices.Contracts
{
    public interface IAddressService
    {
        public Task AddAddressAsync(AddressViewModel model, Guid userId);
        public Task EditAddressAsync(AddressViewModel model, Guid userId);
        public Task<AddressViewModel> GetAddressByIdAsync(Guid id);
        public Task RemoveOtherIsDefaultProp(Guid userId);
    }
}
