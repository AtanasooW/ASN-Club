using ASNClub.ViewModels.Product;

namespace ASNClub.Services.Models
{
    public class AllProductsSortedModel
    {
        public AllProductsSortedModel()
        {
            this.Products = new HashSet<ProductAllViewModel>();
        }
        public int TotalProducts { get; set; }
        public IEnumerable<ProductAllViewModel> Products { get; set; }
    }
}