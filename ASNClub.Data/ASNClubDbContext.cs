using ASNClub.Data.Models;
using ASNClub.Data.Models.Adress;
using ASNClub.Data.Models.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection;

namespace ASNClub.Data
{

    public class ASNClubDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ASNClubDbContext(DbContextOptions<ASNClubDbContext> options)
            : base(options)
        {
        }
        //Address
        public DbSet<Address> Addresses { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;

        //Product
        public DbSet<Models.Product.Color> Colors { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Discount> Discounts { get; set; } = null!;
        public DbSet<ImgUrl> ImgUrls { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImgUrl> ProductsImgUrls { get; set; } = null!;
        public DbSet<Models.Product.Type> Types { get; set; } = null!;

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.EnableSensitiveDataLogging();
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ProductImgUrl>(x =>
            x.HasKey(x => new {x.ProductId,x.ImgUrlId})
            );

            Assembly configAssembly = Assembly.GetAssembly(typeof(ASNClubDbContext)) ??
                                      Assembly.GetExecutingAssembly();
            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);

        }
    }
}