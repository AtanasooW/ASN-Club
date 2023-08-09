using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.Data.Models.AddressModels;

namespace ASNClub.Data.Models.Orders
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey("ShippingAdress")]
        public Guid ShippingAdressId { get; set; }
        public Address ShippingAdress { get; set; } = null!;

        [Required]
        public decimal OrderTotal => ShoppingCart.ShoppingCartItems.Sum(x => x.Product.Price);

        [Required]
        [ForeignKey("OrderStatus")]
        public Guid OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;

        [Required]
        [ForeignKey("ShoppingCart")]
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;
    }
}
