using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.WishListServices
{
    public interface IWishListService
    {
        Task<ApiResponse<string>> AddOrRemove(int u_id, int Pro_id);
        Task<ApiResponse<string>> RemoveFromWishList(int u_id, int pro_id);
        Task<List<WishListViewDto>> GetAllWishItems(int u_id);

    }
}
