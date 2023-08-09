using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASNClub.Data.Models.AddressModels;

namespace ASNClub.Data.Configurations
{
    public class CountryEntityConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(this.GenerateCountries());
        }
        private Country[] GenerateCountries()
        {
            ICollection<Country> countries = new HashSet<Country>();

            Country country;

            country = new Country()
            {
                Id = 1,
                Name = "Bulgaria"
            };
            countries.Add(country);

            country = new Country()
            {
                Id = 2,
                Name = "United States"
            };
            countries.Add(country);

            country = new Country()
            {
                Id = 3,
                Name = "Germany"
            };
            countries.Add(country);

            return countries.ToArray();
        }
    }
}
