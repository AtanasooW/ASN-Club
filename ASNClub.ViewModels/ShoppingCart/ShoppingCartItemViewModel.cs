using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.ViewModels.Discount;

namespace ASNClub.ViewModels.ShoppingCart
{
    public class ShoppingCartItemViewModel
    {
        public int Id { get; set; }
        public Guid ShoppingCartId { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string Make { get; set; } = null!;
        public string Model { get; set; } = null!;

        public string Type { get; set; } = null!;
        public decimal Price { get; set; }
        public ProductDiscountFormModel Discount { get; set; } = null!;
        public string Material { get; set; } = null!;
        public string ImgUrl { get; set; } = null!;
        public string? Color { get; set; } = null!;
        public int ProductQuantity { get; set; }
    }
}
