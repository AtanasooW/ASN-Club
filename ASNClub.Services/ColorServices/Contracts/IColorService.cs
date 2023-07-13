using ASNClub.ViewModels.Color;

namespace ASNClub.Services.ColorServices.Contracts
{
    public interface IColorService
    {
        public Task<IEnumerable<ProductColorFormModel>> AllColorsAsync();
    }
}
