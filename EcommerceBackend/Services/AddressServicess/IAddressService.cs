using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.AddressServicess
{
    public interface IAddressService
    {
        Task<bool> AddnewAddress(int userId, AddNewAddressDto address);
        Task<List<GetAddressDto>> GetAddress(int userId);
        Task<bool> RemoveAddress(int addId);
    }
}
