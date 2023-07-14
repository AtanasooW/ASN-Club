using ASNClub.ViewModels.Type;

namespace ASNClub.Services.TypeServices.Contracts
{
    public interface ITypeService
    {
        public Task<IEnumerable<ProductTypeFormModel>> AllTypesAsync();
        public Task<IEnumerable<string>> AllTypeNamesAsync();

    }
}
