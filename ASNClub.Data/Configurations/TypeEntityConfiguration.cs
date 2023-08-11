using ASNClub.Data.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Configurations
{
    public class TypeEntityConfiguration : IEntityTypeConfiguration<Models.Product.Type>
    {
        public void Configure(EntityTypeBuilder<Models.Product.Type> builder)
        {
            builder.HasData(this.GenerateType());
        }

        private Models.Product.Type[] GenerateType()
        {
            ICollection<Models.Product.Type> types = new HashSet<Models.Product.Type>();

            Models.Product.Type type;

            type = new Models.Product.Type()
            {
                Id = 1,
                Name = "Grips"
            };
            types.Add(type);

            type = new Models.Product.Type()
            {
                Id = 2,
                Name = "Magazine bottom"
            };
            types.Add(type);

            type = new Models.Product.Type()
            {
                Id = 3,
                Name = "Kits"
            };
            types.Add(type);

            return types.ToArray();
        }
    }
}
