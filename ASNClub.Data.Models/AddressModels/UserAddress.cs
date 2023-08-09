using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Models.AddressModels
{
    public class UserAddress
    {
        [Required]
        [ForeignKey("User")]
        public Guid UserId {  get; set; }
        public ApplicationUser User { get; set; } = null!;
        [Required]
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; } = null!;
        
    }
}
