using System.ComponentModel.DataAnnotations;

namespace ASNClub.Data.Models.Orders
{
   public class OrderStatus
   {
       [Key]
       public int Id { get; set; }
   
       [Required]
       public string Status { get; set; } = null!;
   }
}