using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.ProductServices
{
    public interface IProductService
    {
        Task AddProduct(AddProductDto addpro, IFormFile image);
        Task<List<ProductWithCategoryDto>> GetProducts();
        Task<List<ProductWithCategoryDto>> FeaturedPro();
        Task<ProductWithCategoryDto> GetProductById(int id);
        Task<List<ProductWithCategoryDto>> GetProductsByCategoryName(string categoryname);
        Task<bool> DeleteProduct(int id);
        Task UpdateProduct(int id, AddProductDto addpro, IFormFile image);
        Task<List<ProductWithCategoryDto>> SearchProduct(string search);
        Task<List<ProductWithCategoryDto>> HotDeals();
    }
}
