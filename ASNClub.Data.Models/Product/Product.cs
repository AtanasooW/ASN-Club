using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace ASNClub.Data.Models.Product
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Make { get; set; } = null!;
        [Required]
        public string Model { get; set; } = null!;

        [Required]
        [ForeignKey("Type")]
        public int TypeId { get; set; }
        public Type Type { get; set; } = null!; //Models.Type

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey("Discount")]
        public int DiscountId { get; set; }
        public Discount Discount { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        public ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        [Required]
        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; } = null!;
        public ICollection<ProductImgUrl> ImgUrls { get; set; } = new HashSet<ProductImgUrl>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        [ForeignKey("Color")]
        public int? ColorId { get; set; }
        public Color Color { get; set; } = null!;//Models.Color
    }
}