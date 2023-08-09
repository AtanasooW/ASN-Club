using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASNClub.Common.EntityValidationConstants.Adress;
using ASNClub.ViewModels.Country;

namespace ASNClub.ViewModels.Address
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public bool IsDefault { get; set; }
        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(StreetMaxLength, MinimumLength = StreetMinLength)]
        public string Street1 { get; set; } = null!;

        [StringLength(StreetMaxLength, MinimumLength = StreetMinLength)]
        public string? Street2 { get; set; }

        [Required]
        public string StreetNumber { get; set; } = null!;

        [Required]
        public string PostalCode { get; set; } = null!;

        public int CountryId { get; set; }
        public string? Country { get; set; }
        public ICollection<CountryViewModel> Countries { get; set; } = new HashSet<CountryViewModel>();
    }
}
