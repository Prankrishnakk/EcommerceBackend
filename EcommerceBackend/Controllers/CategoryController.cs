using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("getCategories")]
        public async Task<IActionResult> GetCat()

        {
            try
            {
                var categoryList = await _categoryService.GetCategories();
                return Ok(new ApiResponse<IEnumerable<CategoryDto>>(true, "categories fetched", categoryList, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddCategory")]

        public async Task<IActionResult> Add_category([FromForm]CatAddDto newcatgory)
        {
            try
            {
                var res = await _categoryService.AddCategory(newcatgory);

                return Ok(res);

            }
            catch (Exception exc)
            {
                return StatusCode(500, exc.Message);

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteCategory{id}")]

        public async Task<IActionResult> Delete_Cat([FromForm]int id)
        {
            try
            {
                var res = await _categoryService.RemoveCategory(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}