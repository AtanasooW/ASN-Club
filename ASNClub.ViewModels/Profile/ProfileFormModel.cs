using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASNClub.Common.EntityValidationConstants.User;


namespace ASNClub.ViewModels.Profile
{
    public class ProfileFormModel
    {
        public Guid Id { get; set; }
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]

        public string? FirstName { get; set; }
        [StringLength(SurNameMaxLength, MinimumLength = SurNameMinLength)]
        public string? Surname { get; set; }

        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

    }
}
