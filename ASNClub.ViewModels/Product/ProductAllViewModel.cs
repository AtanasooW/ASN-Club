using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.ViewModels.Product
{
    public class ProductAllViewModel
    {
        public string Id { get; set; } = null!;
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal Price { get; set; }
        public double? Rating { get; set; }
        public string ImgUrl { get; set; } = null!;
        [Display(Name = "Type of product")]
        public string Type { get; set; } = null!;
        public string? Color { get; set; }
        public bool IsDiscount { get; set; }
        public double? DiscountRate { get; set; }
        public int? QuantityFromShoppingCart { get;set; }
    }
}
