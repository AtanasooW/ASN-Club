using ASNClub.Data.Models.AddressModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static ASNClub.Common.EntityValidationConstants.User;


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
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]

        public string? FirstName { get; set; }
        [StringLength(SurNameMaxLength, MinimumLength = SurNameMinLength)]
        public string? Surname { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; } = new HashSet<UserAddress>();
    }
}
