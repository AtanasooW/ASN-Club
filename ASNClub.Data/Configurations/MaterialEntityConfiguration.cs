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
    internal class MaterialEntityConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasData(this.GenerateCategories());
        }

        private Material[] GenerateCategories()
        {
            ICollection<Material> categories = new HashSet<Material>();

            Material category;

            category = new Material()
            {
                Id = 1,
                Name = "Wood"
            };
            categories.Add(category);

            category = new Material()
            {
                Id = 2,
                Name = "Aluminum"
            };
            categories.Add(category);
            category = new Material()
            {
                Id = 3,
                Name = "Wood and Aluminum"
            };
            categories.Add(category);

            return categories.ToArray();
        }
    }
}
