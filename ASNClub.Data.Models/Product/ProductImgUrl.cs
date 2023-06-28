using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASNClub.Data.Models.Product
{
    public class ProductImgUrl
    {
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Required]
        [ForeignKey("ImgUrl")]
        public int ImgUrlId { get; set; }
        public ImgUrl ImgUrl { get; set; } = null!;
    }
}