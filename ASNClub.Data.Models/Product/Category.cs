using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Product
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}