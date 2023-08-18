using ASNClub.Data;
using ASNClub.Data.Models;
using ASNClub.Data.Models.AddressModels;
using ASNClub.Data.Models.Orders;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CountyServices;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.OrderServices.Contracts;
using ASNClub.Services.ProfileServices.Contracts;
using ASNClub.Services.ShoppingCartServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Order;
using ASNClub.ViewModels.Product;
using ASNClub.ViewModels.Profile;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ASNClub.Common.EntityValidationConstants;

namespace ASNClub.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly ASNClubDbContext dbContext;
        private readonly ICountryService countryService;
        private readonly IShoppingCartService shoppingCartService;
        private readonly IAddressService addressService;
        private readonly IProfileService profileService;
        public OrderService(ASNClubDbContext _dbContext,
            ICountryService _countryService,
            IShoppingCartService _shoppingCartService,
            IAddressService _addressService,
            IProfileService _profileService)
        {
            dbContext = _dbContext;
            countryService = _countryService;
            shoppingCartService = _shoppingCartService;
            addressService = _addressService;
            profileService = _profileService;
        }


        public async Task<MyOrderDetailsViewModel?> GetMyOrderDetailsByIdAsync(Guid userId, Guid id)
        {
            return await dbContext.Orders
                .Include(x => x.OrderStatus)
                .Include(x=> x.Items)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x => x.Color)
                .Include(x=> x.Items)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x=> x.Type)
                .Include(x=> x.Items)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x=> x.Material)
                .Include(x=> x.Items)
                .ThenInclude(x=> x.Product)
                .ThenInclude(x=> x.ImgUrls)
                .ThenInclude(x=> x.ImgUrl)
                .Where(x => x.UserId == userId && x.Id == id)
                .Select(x => new MyOrderDetailsViewModel
                {
                    Order = new MyOrderViewModel
                    {
                        Id = x.Id,
                        OrderDate = x.OrderDate,
                        OrderStatus = x.OrderStatus.Status,
                        OrderTotal = x.OrderTotal,
                        UserId = userId,
                    },
                    Products = x.Items.Select(x=> new ProductAllViewModel
                    {
                        Id = x.Product.Id.ToString(),
                        Make = x.Product.Make,
                        Model = x.Product.Model,
                        Color = x.Product.Color.Name,
                        Type = x.Product.Type.Name,
                        Material = x.Product.Material.Name,
                        Price = x.Product.Price,
                        IsDiscount = x.Product.Discount.IsDiscount,
                        DiscountRate = x.Product.Discount.DiscountRate,
                        QuantityFromShoppingCart = x.Quantity,
                        ImgUrl = x.Product.ImgUrls.FirstOrDefault(p=> p.ProductId == x.Product.Id).ImgUrl.Url
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<ICollection<MyOrderViewModel>> GetAllOrdersAsync()
        {
            return await dbContext.Orders.Include(x => x.OrderStatus).Select(x => new MyOrderViewModel
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus.Status,
                OrderTotal = x.OrderTotal,
                UserId = x.UserId
            }).ToListAsync();
        }
        public async Task<ICollection<MyOrderViewModel>> GetMyOrdersByIdAsync(Guid userId)
        {
            return await dbContext.Orders.Where(x => x.UserId == userId).Include(x => x.OrderStatus).Select(x => new MyOrderViewModel
            {
                Id = x.Id,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus.Status,
                OrderTotal = x.OrderTotal,
                UserId = userId
            }).ToListAsync();
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
                        Surname = x.User.Surname,
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
                if (order.ShippingAdress == null)
                {
                    order.ShippingAdress = new AddressViewModel();
                }
                order.ShippingAdress.Countries = await countryService.GetCountryNamesAsync();
                order.Products = await shoppingCartService.GetAllProductsFromShoppingCartAsync(userId);
            }
            return order;
        }

        public async Task PlaceOrderAsync(OrderViewModel model)
        {
            var address = await addressService.GetAddressTypeAddressByIdAsync(model.ShippingAdress.Id);
            var shoppingCart = await shoppingCartService.GetShoppingCartByUserId(model.Profile.Id);
            Order order = new Order();
            if (address == null && shoppingCart == null)
            {//IF person doesnt have a profile
                var address1 = new Address()
                {
                    CountryId = model.ShippingAdress.CountryId,
                    City = model.ShippingAdress.City,
                    Street1 = model.ShippingAdress.Street1,
                    Street2 = model.ShippingAdress.Street2,
                    StreetNumber = model.ShippingAdress.StreetNumber,
                    PostalCode = model.ShippingAdress.PostalCode,
                    IsDefault = false
                };
                await dbContext.Addresses.AddAsync(address1);
                order.ShippingAdressId = address1.Id;
                order.OrderDate = DateTime.Now;
                order.OrderStatusId = 1;
                await dbContext.Orders.AddAsync(order);
                await dbContext.SaveChangesAsync();

                OrderItem item = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = (int)model.ProductId,
                    Quantity = (int)model.Quantity,
                };
                order.Items.Add(item);
                await dbContext.SaveChangesAsync();
                return;
            }
            else
            {//if person have profile
                if (!shoppingCart.ShoppingCartItems.Any())
                {
                    throw new InvalidOperationException("No items in the cart");
                }
                var profile = await profileService.GetProfileOfTypeProfileByIdAsync(model.Profile.Id);
                CompareAddreses(model, address,profile);
                CompareProfileInfo(model, profile);

                order.OrderDate = DateTime.Now;
                order.OrderStatusId = 1;
                order.ShippingAdressId = address.Id;
                order.UserId = model.Profile.Id;
                order.ShoppingCartId = shoppingCart.Id;
            }
            await dbContext.Orders.AddAsync(order);
            await dbContext.SaveChangesAsync();
            order.Items = shoppingCart.ShoppingCartItems.Select(x => new OrderItem()
            {
                OrderId = order.Id,
                ProductId = x.ProductId,
                Quantity = x.Quantity,
            }).ToList();
            foreach (var item in shoppingCart.ShoppingCartItems)
            {
                if (item.Product.Discount.IsDiscount)
                {
                    order.OrderTotal += (item.Product.Price - ((item.Product.Price * (decimal)item.Product.Discount.DiscountRate) / 100))  * item.Quantity;
                }
                else
                {
                    order.OrderTotal += item.Product.Price * item.Quantity;
                }
            }
            foreach (var item in shoppingCart.ShoppingCartItems)
            {
                dbContext.ShoppingCartItems.Remove(item);
            }
            await dbContext.SaveChangesAsync();
        }

        private async void CompareProfileInfo(OrderViewModel model, ApplicationUser profile)
        {
           
            if (model.Profile.FirstName != profile.FirstName)
            {
                profile.FirstName = model.Profile.FirstName;
            }
            if (model.Profile.Surname != profile.Surname)
            {
                profile.Surname = model.Profile.Surname;
            }
            if (model.Profile.PhoneNumber != profile.PhoneNumber)
            {
                profile.PhoneNumber = model.Profile.PhoneNumber;
            }
            if (model.Profile.Email != profile.Email)
            {
                profile.Email = model.Profile.Email;
            }
            await dbContext.SaveChangesAsync();
        }

        private async void CompareAddreses(OrderViewModel model, Address address, ApplicationUser profile)
        {
            if (address == null)
            {
                CreateAddressAsync(model,profile);
                return;

            }
            if (model.ShippingAdress.CountryId != address.Country.Id)
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

        private async void CreateAddressAsync(OrderViewModel model, ApplicationUser profile)
        {
            Address address = new Address()
            {
                CountryId = model.ShippingAdress.CountryId,
                City = model.ShippingAdress.City,
                PostalCode = model.ShippingAdress.PostalCode,
                Street1 = model.ShippingAdress.Street1,
                Street2 = model.ShippingAdress.Street2,
                StreetNumber = model.ShippingAdress.StreetNumber,
                IsDefault = true,
            };
            await dbContext.Addresses.AddAsync(address);
            await dbContext.SaveChangesAsync();

            UserAddress userAddress = new UserAddress()
            {
                AddressId = address.Id,
                UserId = profile.Id
            };
            await dbContext.UsersAddresses.AddAsync(userAddress);
            profile.UserAddresses.Add(userAddress);
            await dbContext.SaveChangesAsync();
        }

        public async Task<MyOrderDetailsViewModel?> GetOrderDetailByIdAsync(Guid id)
        {
            return await dbContext.Orders
                .Include(x => x.OrderStatus)
                .Include(x=> x.User)
                .ThenInclude(x=> x.UserAddresses)
                .ThenInclude(x=> x.Address)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Color)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Type)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Material)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.ImgUrls)
                .ThenInclude(x => x.ImgUrl)
                .Where(x => x.Id == id)
                .Select(x => new MyOrderDetailsViewModel
                {
                    Order = new MyOrderViewModel
                    {
                        Id = x.Id,
                        OrderDate = x.OrderDate,
                        OrderStatus = x.OrderStatus.Status,
                        OrderTotal = x.OrderTotal,
                        UserId = x.UserId,
                    },
                    Products = x.Items.Select(x => new ProductAllViewModel
                    {
                        Id = x.Product.Id.ToString(),
                        Make = x.Product.Make,
                        Model = x.Product.Model,
                        Color = x.Product.Color.Name,
                        Type = x.Product.Type.Name,
                        Material = x.Product.Material.Name,
                        Price = x.Product.Price,
                        IsDiscount = x.Product.Discount.IsDiscount,
                        DiscountRate = x.Product.Discount.DiscountRate,
                        QuantityFromShoppingCart = x.Quantity,
                        ImgUrl = x.Product.ImgUrls.FirstOrDefault(p => p.ProductId == x.Product.Id).ImgUrl.Url
                    }).ToList(),
                    FirstName = x.User.FirstName,
                    SurnameName = x.User.Surname,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    ShippingAddress = x.ShoppingCart.User.UserAddresses.Where(x => x.Address.IsDefault).Select(x=> new AddressViewModel
                    {
                        Country = x.Address.Country.Name,
                        City = x.Address.City,
                        Street1 = x.Address.Street1,
                        Street2 = x.Address.Street2,
                        StreetNumber = x.Address.StreetNumber,
                        PostalCode = x.Address.PostalCode,
                       
                    }).FirstOrDefault()
                }).FirstOrDefaultAsync();
        }

        public async Task<OrderStatusViewModel> GetOrderStatusAsync(Guid id)
        {
            var orderStatuses = await dbContext.OrdersStatuses.Select(x => new OrderStatusModel
            {
               Id = x.Id,
               Status = x.Status
            }).ToListAsync();
            OrderStatusViewModel model = new OrderStatusViewModel()
            {
                OrderStatuses = orderStatuses,
               OrderId = id,
            };
            return model;
        }

        public async Task EditOrderStatusAsync(OrderStatusViewModel model)
        {
            var order = await dbContext.Orders.Where(x => x.Id == model.OrderId).FirstOrDefaultAsync();
            order.OrderStatusId = (int)model.OrderStatusId;
            await dbContext.SaveChangesAsync();
        }
    }
}
