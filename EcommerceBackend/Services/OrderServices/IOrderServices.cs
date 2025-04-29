using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.OrderServices
{
    public interface IOrderServices
    {
        Task<bool> CrateOrder_CheckOut(int userId, CreateOrderDto createOrderDto);
        Task<bool> Indidvidual_ProductBuy(int userId, int productId, CreateOrderDto order_Dto);
        Task<List<OrderviewDto>> GetOrderDetails(int userId);
        Task<List<OrderAdminViewDto>> GetOrderDetailsAdmin();
        Task<decimal> TotalRevenue();
        Task<int> TotalProductsPurchased();
        Task<List<OrderviewDto>> GetOrderDetailsAdmin_byuserId(int userId);
        Task<string> UpdateOrderStatus(int oId);
    }
}
