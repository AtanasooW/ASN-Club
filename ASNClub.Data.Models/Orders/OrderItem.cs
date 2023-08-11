using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.Data.Models.Product;
        using static ASNClub.Common.EntityValidationConstants.ShoppingCartItem;

namespace ASNClub.Data.Models.Orders
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product.Product Product { get; set; } = null!;

        [Required]
        [Range(QuantityMinCount, QuantityMaxCount)]
        public int Quantity { get; set; }
    }
}
