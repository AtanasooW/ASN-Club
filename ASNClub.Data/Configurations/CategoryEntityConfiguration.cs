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
    internal class CategoryEntityConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasData(this.GenerateCategories());
        }

        private Color[] GenerateCategories()
        {
            ICollection<Color> categories = new HashSet<Color>();

            Color category;

            category = new Color()
            {
                Id = 1,
                Name = "Wooden"
            };
            categories.Add(category);

            category = new Color()
            {
                Id = 2,
                Name = "Aluminum"
            };
            categories.Add(category);

            return categories.ToArray();
        }
    }
}
