using ASNClub.Data;
using ASNClub.Data.Models.AddressModels;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.AddressServices
{
    public class AddressService : IAddressService
    {
        readonly private ASNClubDbContext dbContext;
        public AddressService(ASNClubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task AddAddressAsync(AddressViewModel model, Guid userId)
        {
            if (!dbContext.UsersAddresses.Where(x => x.UserId == userId).Any())
            {
                model.IsDefault = true;
            }
            Address address = new Address()
            {
                CountryId = model.CountryId,
                City = model.City,
                PostalCode = model.PostalCode,
                Street1 = model.Street1,
                Street2 = model.Street2,
                StreetNumber = model.StreetNumber,
                IsDefault = model.IsDefault,
            };

            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();
            UserAddress userAddress = new UserAddress()
            {
                AddressId = address.Id,
                UserId = userId
            };

            await dbContext.UsersAddresses.AddAsync(userAddress);
            await dbContext.SaveChangesAsync();
        }

        public async Task EditAddressAsync(AddressViewModel model, Guid userId)
        {
            var address = await dbContext.Addresses.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (address == null)
            {
                throw new InvalidOperationException("Invalid address");
            }
            address.CountryId = model.CountryId;
            address.City = model.City;
            address.PostalCode = model.PostalCode;
            address.Street1 = model.Street1;
            address.Street2 = model.Street2;
            address.StreetNumber = model.StreetNumber;
            address.IsDefault = model.IsDefault;
            await dbContext.SaveChangesAsync();
        }

        public async Task<AddressViewModel?> GetAddressByIdAsync(Guid id)
        {
            return await dbContext.UsersAddresses.Where(x => x.AddressId == id)
                .Select(x => new AddressViewModel
                {
                    Id = x.AddressId,
                    IsDefault = x.Address.IsDefault,
                    Country = x.Address.Country.Name,
                    CountryId = x.Address.CountryId,
                    City = x.Address.City,
                    PostalCode = x.Address.PostalCode,
                    Street1 = x.Address.Street1,
                    Street2 = x.Address.Street2,
                    StreetNumber = x.Address.StreetNumber,
                }).FirstOrDefaultAsync();
        }

        public async Task<Address?> GetAddressTypeAddressByIdAsync(Guid id)
        {
            return await dbContext.Addresses.Include(x=> x.Country).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<AddressViewModel?> GetShippingAddressByIdAsync(Guid userId)
        {
            return await dbContext.UsersAddresses.Where(x => x.UserId == userId && x.Address.IsDefault)
                .Select(x => new AddressViewModel
                {
                    Id = x.AddressId,
                    IsDefault = x.Address.IsDefault,
                    Country = x.Address.Country.Name,
                    CountryId = x.Address.CountryId,
                    City = x.Address.City,
                    PostalCode = x.Address.PostalCode,
                    Street1 = x.Address.Street1,
                    Street2 = x.Address.Street2,
                    StreetNumber = x.Address.StreetNumber,
                }).FirstOrDefaultAsync();
        }

        public async Task RemoveOtherIsDefaultProp(Guid userId)
        {
            var addresses = await dbContext.UsersAddresses.Where(x=> x.UserId == userId).Select(x=> x.Address).ToListAsync();
            foreach (var address in addresses)
            {
                address.IsDefault = false;
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task SetDefaultAddressAsync(Guid addressId, Guid userId)
        {
            await RemoveOtherIsDefaultProp(userId);
            var address = await dbContext.Addresses.Where(x=> x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
            {
                throw new InvalidOperationException("Invalid address");
            }
            address.IsDefault = true;
            await dbContext.SaveChangesAsync();
        }
    }
}
