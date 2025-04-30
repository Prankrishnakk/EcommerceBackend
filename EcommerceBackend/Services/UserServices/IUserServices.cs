using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.UserServices
{
    public interface IUserServices
    {
        Task<List<UserViewDto>> ListUsers();
        Task<UserViewDto> GetUser(int id);
        Task<BlockUnblockDto> BlockUnblockUser(int id);
    }
}
