﻿using ASNClub.Data.Models.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Configurations
{
    public class ColorEntityConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.HasData(this.GenerateColors());
        }
        private Color[] GenerateColors()
        {
            ICollection<Color> colors = new HashSet<Color>();

            Color color;

            color = new Color()
            {
                Id = 1,
                Name = "Black"
            };
            colors.Add(color);

            color = new Color()
            {
                Id = 2,
                Name = "Red"
            };
            colors.Add(color);

            return colors.ToArray();
        }
    }
}
