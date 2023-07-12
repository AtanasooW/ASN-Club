using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Product
{
    public class ProductFormModel
    {
        [Required]
        public string Make { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;

        [Required]
        [Display(Name = "Type")]
        public int TypeId { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Discount")]
        public int DiscountId { get; set; }

        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required]
        public string ImgUrl { get; set; } = null!;

        [Display(Name = "Color")]
        public int? ColorId { get; set; }
    }
}
