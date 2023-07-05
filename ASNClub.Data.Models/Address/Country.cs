using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Adress
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
    }
}