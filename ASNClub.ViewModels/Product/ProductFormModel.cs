using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.ViewModels.Category;
using ASNClub.ViewModels.Type;
using ASNClub.ViewModels.Color;
using ASNClub.ViewModels.Discount;

namespace ASNClub.ViewModels.Product
{
    public class ProductFormModel
    {
        public ProductFormModel()
        {
            this.Materials = new HashSet<ProductMaterialFormModel>();
            this.Colors = new HashSet<ProductColorFormModel>();
            this.Types = new HashSet<ProductTypeFormModel>();
            this.ImgUrls = new List<string>();
        }
        [Required]
        public string Make { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;

        [Required]
        [Display(Name = "Type")]
        public int TypeId { get; set; }

        public IEnumerable<ProductTypeFormModel> Types { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Discount")]
        public ProductDiscountFormModel Discount { get; set; }

        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<ProductMaterialFormModel> Materials { get; set; }
        [Required]
        public List<string> ImgUrls { get; set; } = null!;

        [Display(Name = "Color")]
        public int? ColorId { get; set; }

        public IEnumerable<ProductColorFormModel> Colors { get; set; }
    }
}
