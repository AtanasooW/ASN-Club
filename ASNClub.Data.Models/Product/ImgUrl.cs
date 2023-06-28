using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Product
{
    public class ImgUrl
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; } = null!;
    }
}