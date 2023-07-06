using ASNClub.Data.Models.Adress;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Models
{
    /// <summary>
    /// This is custom User based on the Identity User class
    /// Its modified i litle bit (Added address to the user)
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; } = null!;
    }
}
