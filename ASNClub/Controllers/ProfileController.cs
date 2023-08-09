using ASNClub.Infrastructure.Extensions;
using ASNClub.Services.AddressServices.Contracts;
using ASNClub.Services.CountyServices.Contracts;
using ASNClub.Services.ProfileServices.Contracts;
using ASNClub.ViewModels.Address;
using ASNClub.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ASNClub.Controllers
{
    public class ProfileController : Controller
    {
        readonly private IProfileService profileService;
        readonly private IAddressService addressService;
        readonly private ICountryService countryService;
        public ProfileController(IProfileService _profileService, IAddressService _addressService, ICountryService _countryService)
        {
            profileService = _profileService;
            addressService = _addressService;
            countryService = _countryService;
        }
        public async Task<IActionResult> MyProfile()
        {
            var userId = User.GetId();
            var user = await profileService.GetProfileByIdAsync(Guid.Parse(userId));
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile(Guid id)
        {
            var model = await profileService.GetProfileByIdForEditAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await profileService.EditProfileAsync(model);
            return RedirectToAction("MyProfile");
        }
        [HttpGet]
        public async Task<IActionResult> EditAddress(Guid id)
        {
            var model = await addressService.GetAddressByIdAsync(id);
            model.Countries = await countryService.GetCountryNamesAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await countryService.GetCountryNamesAsync();
                return View(model);
            }
            var userId = User.GetId();
            if (model.IsDefault)
            {
                await addressService.RemoveOtherIsDefaultProp(Guid.Parse(userId));
            }
            await addressService.EditAddressAsync(model,Guid.Parse(userId));
            return RedirectToAction("MyProfile");
        }
        [HttpGet]
        public async Task<IActionResult> AddAddress()
        {
            var model = new AddressViewModel();
            model.Countries = await countryService.GetCountryNamesAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Countries = await countryService.GetCountryNamesAsync();
                return View(model);
            }
            var userId = User.GetId();
            if (model.IsDefault)
            {
                await addressService.RemoveOtherIsDefaultProp(Guid.Parse(userId));
            }
            await addressService.AddAddressAsync(model, Guid.Parse(userId));
            return RedirectToAction("MyProfile");
        }
    }
}
