using ASNClub.Data.Models;
using ASNClub.ViewModels.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASNClub.Services.ProfileServices.Contracts
{
    public interface IProfileService
    {
        public Task EditProfileAsync(ProfileFormModel model);
        public Task<ProfileViewModel> GetProfileByIdAsync(Guid id);
        public Task<ApplicationUser> GetProfileOfTypeProfileByIdAsync(Guid id);
        public Task<ProfileFormModel> GetProfileByIdForEditAsync(Guid id);
    }
}
