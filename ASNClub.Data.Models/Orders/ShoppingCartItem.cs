namespace ASNClub.Data.Models.Orders
{
    using Models.Product;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Common.EntityValidationConstants.ShoppingCartItem;
    /// <summary>
    /// Table for the products for the shopping cart
    /// </summary>
    public class ShoppingCartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ShoppingCart")]
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = null!;

        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        [Range(QuantityMinCount,QuantityMaxCount)]
        public int Quantity { get; set; }
    }
}
