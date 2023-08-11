using ASNClub.Data;
using ASNClub.Data.Models.Product;
using ASNClub.Services.ProfileServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        readonly private ASNClubDbContext dbContext;
        public ProfileService(ASNClubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task EditProfileAsync(ProfileFormModel model)
        {
            var product = await dbContext.Users.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (product == null)
            {
                throw new InvalidOperationException("Invalid product");
            }

            product.FirstName = model.FirstName;
            product.Surname = model.Surname;
            product.Email = model.Email;
            product.PhoneNumber = model.PhoneNumber;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ProfileViewModel?> GetProfileByIdAsync(Guid id)
        {
            return await dbContext.Users.Where(x => x.Id == id).Select(x => new ProfileViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                Surname = x.Surname,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Addresses = x.UserAddresses.Count >= 1 ? x.UserAddresses.Select(x => new AddressViewModel
                {
                    Id = x.AddressId,
                    IsDefault = x.Address.IsDefault,
                    Country = x.Address.Country.Name,
                    City = x.Address.City,
                    PostalCode = x.Address.PostalCode,
                    Street1 = x.Address.Street1,
                    Street2 = x.Address.Street2,
                    StreetNumber = x.Address.StreetNumber

                }).ToList() : null
            }).FirstOrDefaultAsync();
        }

        public async Task<ProfileFormModel?> GetProfileByIdForEditAsync(Guid id)
        {
            return await dbContext.Users.Where(x => x.Id == id).Select(x => new ProfileFormModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                Surname = x.Surname,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            }).FirstOrDefaultAsync();
        }
    }
}
