using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Product
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool IsDiscount { get; set; } = false;
        [Required]
        public double DiscountRate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}