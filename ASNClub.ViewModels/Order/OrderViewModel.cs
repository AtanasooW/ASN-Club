using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.Profile;

namespace ASNClub.ViewModels.Order
{
    public class OrderViewModel
    {
        public ProfileViewModel Profile { get; set; } = null!;
        public ICollection<ProductAllViewModel> Products { get; set; } = new HashSet<ProductAllViewModel>();
        public AddressViewModel ShippingAdress { get; set; } = null!;

    }
}
