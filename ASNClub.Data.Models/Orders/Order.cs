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

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [ForeignKey("ShippingAdress")]
        public Guid ShippingAdressId { get; set; }
        public Address ShippingAdress { get; set; } = null!;

        [Required]
        public decimal OrderTotal { get; set; }

        [Required]
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;

        [ForeignKey("ShoppingCart")]
        public Guid? ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set;} = new HashSet<OrderItem>();
    }
}
