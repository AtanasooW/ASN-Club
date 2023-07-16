using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.ViewModels.Discount;

namespace ASNClub.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
            //this.ImgUrls = new HashSet<string>();
        }
        public int Id { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductDiscountFormModel Discount { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Quantity { get; set; }
        public string Category { get; set; } = null!;
        public string ImgUrl { get; set; } //FIX IT TO COllection
        public string? Color { get; set; }
    }
}
