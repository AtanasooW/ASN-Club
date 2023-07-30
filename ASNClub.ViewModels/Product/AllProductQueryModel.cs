using System.ComponentModel.DataAnnotations;

using ASNClub.ViewModels.Product.Enums;
using static ASNClub.Common.ApplicationConstants;

namespace ASNClub.ViewModels.Product
{
    public class AllProductQueryModel
    {
        public AllProductQueryModel()
        {
           this.Makes = new HashSet<string>();
           this.Models = new HashSet<string>();
           this.Types = new HashSet<string>();
           this.Categories = new HashSet<string>();

           this.Products = new HashSet<ProductAllViewModel>();
            this.ProductsPerPage = DefaultEntitiesPerPage;
            this.CurrentPage = DefaultPage;
        }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Type { get; set; }
        public string? Material { get; set; }

        [Display(Name = "Search by text")]
        public string? SearchString { get; set; }

        [Display(Name = "Sort Product By")]
        public ProductSorting ProductSorting { get; set; }

        public int CurrentPage { get; set; }

        [Display(Name = "Show Products On Page")]
        public int ProductsPerPage { get; set; }

        public int TotalProducts { get; set; }

        public IEnumerable<string> Makes { get; set; }
        public IEnumerable<string>? Models { get; set; }
        public IEnumerable<string> Types { get; set; }
        public IEnumerable<string> Categories { get; set; }

        public IEnumerable<ProductAllViewModel> Products { get; set; }
    }
}
