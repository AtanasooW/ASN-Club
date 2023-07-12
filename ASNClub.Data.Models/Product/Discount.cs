using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Product
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool IsDiscount { get; set; } = false;
        
        public double? DiscountRate { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
    }
}