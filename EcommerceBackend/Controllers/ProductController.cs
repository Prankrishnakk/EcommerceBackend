using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _Services;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService services, ILogger<ProductController> logger)
        {
            _Services = services;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Add_Pro")]
        public async Task<IActionResult> AddPro([FromForm] AddProductDto new_pro, IFormFile image)
        {
            try
            {

                if (new_pro == null || image == null)
                {
                    return BadRequest("Invalid product data or image file.");
                }


                if (image.Length > 10485760)
                {
                    return BadRequest("File size exceeds the 10 MB limit.");
                }

                if (!image.ContentType.StartsWith("image/"))
                {
                    return BadRequest("Invalid file type. Only image files are allowed.");
                }

                await _Services.AddProduct(new_pro, image);
                return Ok(new ApiResponse<string>(true, "Product added successfully!", null, null));
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while adding a product.");


                return StatusCode(500, ex.InnerException?.Message);
            }
        }



        [HttpGet("All")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _Services.GetProducts();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Products fetched ", products, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }






        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var p = await _Services.GetProductById(id);

                if (p == null)
                {
                    return Ok(new ApiResponse<string>(false, "Product not found", " ", null));
                }
                return Ok(new ApiResponse<ProductWithCategoryDto>(true, "Product  found", p, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("Update_Pro/{id}")]
        public async Task<IActionResult> Update_pro(int id, [FromForm] UpdateProductDto updateProduct_Dto, IFormFile? image)
        {
            try
            {
                await _Services.UpdateProduct(id, updateProduct_Dto, image);

                return Ok(new ApiResponse<string>(true, "product updated", null, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePro(int id)
        {
            try
            {
                bool res = await _Services.DeleteProduct(id);
                if (res)
                {
                    return Ok(new ApiResponse<string>(true, "Product deleted", null, null));
                }

                return NotFound(new ApiResponse<string>(false, "Product Not found", null, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search-item")]
        public async Task<IActionResult> SearchPro(string search)
        {
            try
            {
                var res = await _Services.SearchProduct(search);
                if (res == null)
                {
                    return NotFound(new ApiResponse<string>(true, "no products matched", null, null));

                }
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, " products are match with..", res, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpGet("getByCategoryName")]
        public async Task<IActionResult> GetByCateName(string CatName)
        {
            try
            {
                var p = await _Services.GetProductsByCategoryName(CatName);
                if (p == null)
                {
                    return Ok(new ApiResponse<string>(false, "No products in this category", " ", null));
                }
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, " products in this category", p, null));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("FeaturedPro")]
        public async Task<IActionResult> FeturedPro()
        {
            try
            {
                var res = await _Services.FeaturedPro();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Product fetched", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("HotDeals")]
        public async Task<IActionResult> HotDeals()
        {
            try
            {
                var res = await _Services.HotDeals();
                return Ok(new ApiResponse<IEnumerable<ProductWithCategoryDto>>(true, "Product fetched", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Paginated")]
        [Authorize]
        public async Task<IActionResult> Pagination([FromQuery] int pageNumber = 1, [FromQuery] int pagesize = 10)
        {
            try
            {
                var products = await _Services.PaginatedProduct(pageNumber, pagesize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }





    }
}
