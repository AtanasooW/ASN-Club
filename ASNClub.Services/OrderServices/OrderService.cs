using ASNClub.Data;
using ASNClub.Data.Models.AddressModels;
using ASNClub.Data.Models.Orders;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CountyServices;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Order;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ASNClubDbContext dbContext;
        private readonly ICountryService countryService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAddressService addressService;
        public OrderService(ASNClubDbContext _dbContext, ICountryService _countryService, IShoppingCartService _shoppingCartService, IAddressService _addressService)
        {
            dbContext = _dbContext;
            countryService = _countryService;
            shoppingCartService = _shoppingCartService;
            addressService = _addressService;
        }

        public async Task<OrderViewModel?> GetOrderByUserIdAsync(Guid userId)
        {
            var shippingAddress = await addressService.GetShippingAddressByIdAsync(userId);
            var order = await dbContext.ShoppingCarts.Where(x => x.UserId == userId)
                .Include(x => x.User)
                .ThenInclude(x => x.UserAddresses)
                .Include(x => x.ShoppingCartItems)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Discount)
                .Select(x => new OrderViewModel
                {
                    Profile = new ProfileViewModel()
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        SurnameName = x.User.SurnameName,
                        Email = x.User.Email,
                        PhoneNumber = x.User.PhoneNumber,
                        Addresses = x.User.UserAddresses.Count >= 1 ? x.User.UserAddresses.Select(x => new AddressViewModel
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
                    },
                    ShippingAdress = shippingAddress
                }).FirstOrDefaultAsync();
            if (order != null)
            {
                order.ShippingAdress.Countries = await countryService.GetCountryNamesAsync();
                order.Products = await shoppingCartService.GetAllProductsFromShoppingCartAsync(userId);
            }
            return order;
        }

        public async Task PlaceOrderAsync(OrderViewModel model)
        {
           
            var address = await addressService.GetAddressTypeAddressByIdAsync(model.ShippingAdress.Id);
            CompareAddreses(model, address);
            Order order = new Order()
            {
                OrderDate = DateTime.Now,
                OrderStatusId = 1,
               ShippingAdressId = address.Id,
               UserId = model.Profile.Id

           }
        }
        private async void CompareAddreses(OrderViewModel model, Address address)
        {
            if (model.ShippingAdress.Country != address.Country.Name)
            {
                address.Country.Name = model.ShippingAdress.Country;
            }
            if (model.ShippingAdress.City != address.City)
            {
                address.City = model.ShippingAdress.City;
            }
            if (model.ShippingAdress.Street1 != address.Street1)
            {
                address.Street1 = model.ShippingAdress.Street1;
            }
            if (model.ShippingAdress.Street2 != address.Street2)
            {
                address.Street2 = model.ShippingAdress.Street2;
            }
            if (model.ShippingAdress.StreetNumber != address.StreetNumber)
            {
                address.StreetNumber = model.ShippingAdress.StreetNumber;
            }
            if (model.ShippingAdress.PostalCode != address.PostalCode)
            {
                address.PostalCode = model.ShippingAdress.PostalCode;
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
