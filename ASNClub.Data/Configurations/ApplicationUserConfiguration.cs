using ASNClub.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Data.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(this.GenerateUsers());
        }
        private ApplicationUser[] GenerateUsers()
        {
            ICollection<ApplicationUser> users = new HashSet<ApplicationUser>();

            ApplicationUser user;

            user = new ApplicationUser()
            {
                Id = Guid.Parse("C708C62D-8C59-4528-AACA-53C413F15F70"),
                Email = "Test@gmail.com",
                NormalizedEmail = "TEST@GMAIL.COM",
                UserName = "Test@gmail.com",
                NormalizedUserName = "TEST@GMAIL.COM",
                PhoneNumber = "8888888888",
                FirstName = "Vaskata",
                SurnameName = "Petrov"

            };
            users.Add(user);
            return users.ToArray();
        }
    }
}
