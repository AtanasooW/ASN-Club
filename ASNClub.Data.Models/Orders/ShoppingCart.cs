using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNClub.Data.Models.Orders
{
    /// <summary>
    /// Table that stores all products in the shopping cart
    /// </summary>
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
    }
}