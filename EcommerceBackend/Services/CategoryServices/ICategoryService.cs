using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategories();
        Task<ApiResponse<CatAddDto>> AddCategory(CatAddDto Addcategories);
        Task<ApiResponse<string>> RemoveCategory(int id);

    }
}
