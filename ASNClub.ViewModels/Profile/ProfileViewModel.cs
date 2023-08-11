using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; } = null!;
        public string? SurnameName { get; set;} = null!;
        public string Email { get; set;} = null!;
        public string PhoneNumber { get; set;} = null!;
        public ICollection<AddressViewModel> Addresses { get; set; } = new HashSet<AddressViewModel>();
        public ICollection<MyOrderViewModel> Orders { get; set; } = new HashSet<MyOrderViewModel>();
    }
}
