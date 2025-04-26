using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.WishListServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishListService _Service;
        public WishlistController(IWishListService Service)
        {
            _Service = Service;
        }
        [HttpGet("Getwishlist")]
        [Authorize]
        public async Task<IActionResult> GetWishList()
        {
            try
            {
                int u_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _Service.GetAllWishItems(u_id);

                if (res.Count >= 0)
                {
                    return Ok(new ApiResponse<IEnumerable<WishListViewDto>>(true, "whishlist fetched", res, null));

                }
                return Ok(new ApiResponse<IEnumerable<WishListViewDto>>(true, "no item in wishlist", res, null));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        [HttpPost("AddOrRemove/{pro_id}")]
        [Authorize]
        public async Task<IActionResult> Add(int pro_id)
        {
            try
            {
                int u_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _Service.AddOrRemove(u_id, pro_id);

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveWishlist/{pro_id}")]
        [Authorize]
        public async Task<IActionResult> Remove( int pro_id)
        {
            try
            {
                int u_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _Service.RemoveFromWishList(u_id, pro_id);

                if (res.IsSuccess)
                {
                    return Ok(res);
                }

                return BadRequest(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}

    

