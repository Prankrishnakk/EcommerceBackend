using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.CartServices
{
    public interface ICartService
    {
        Task<CartwithTotalPrice> GetAllCartItems(int userId);
        Task<ApiResponse<CartViewDto>> AddToCart(int userId, int productId);
        Task<ApiResponse<string>> RemoveFromCart(int userId, int ProductId);
        Task<ApiResponse<CartViewDto>> IncraseQuantity(int userId, int productId);
        Task<ApiResponse<CartViewDto>> DecreaseQuantity(int userId, int ProductId);
    }
}
