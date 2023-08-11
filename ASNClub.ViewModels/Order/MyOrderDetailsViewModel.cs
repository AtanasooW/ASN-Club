using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Order
{
    public class MyOrderDetailsViewModel
    {
        public MyOrderViewModel Order { get; set; }
        public ICollection<ProductAllViewModel> Products { get; set; }
        public AddressViewModel? ShippingAddress { get;set; }
        public string? FirstName { get; set; } = null!;
        public string? SurnameName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
    }
}
