﻿using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.CartServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartServices;
        public CartController(ICartService cartServices)
        {
            _cartServices = cartServices;
        }


        [HttpGet("cartitems")]
        public async Task<IActionResult> GetAllCartItems()
        {
            try
            {

                var userid = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var items = await _cartServices.GetAllCartItems(int.Parse(userid));



                if (items == null)
                {
                    return Ok(new ApiResponse<CartwithTotalPrice>(false, "Cart is Empty", null, "Add some Products"));
                }

                return Ok(new ApiResponse<CartwithTotalPrice>(true, "Cart successfully fetched", items, null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CartwithTotalPrice>(false, ex.Message, null, ex.Message));
            }
        }



        [HttpPost("AddToCart/{pro_id}")]
        public async Task<IActionResult> AddtoCart(int pro_id)
        {
            try
            {
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var res = await _cartServices.AddToCart(user_id, pro_id);

                return Ok(new ApiResponse<CartViewDto>(true, res.Message, res.Data, res.Error));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpDelete("DeleteItemFromCart/{pro_id}")]
        public async Task<IActionResult> Remove_FromCart(int pro_id)
        {
            try
            {
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var data = await _cartServices.RemoveFromCart(user_id, pro_id);

                return Ok(data);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPatch("increment-product-qty/{pro_id}")]
        public async Task<IActionResult> INR_ProQty(int pro_id)
        {
            try
            {
                if (pro_id <= 0)
                {
                    return BadRequest("Invalid product ID.");
                }

                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var data = await _cartServices.IncraseQuantity(user_id, pro_id);


                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred: " + ex.Message);
            }
        }


        [HttpPatch("decrement-product-qty/{pro_id}")]
        public async Task<IActionResult> DCR_ProQty(int pro_id)
        {
            try
            {
                if (pro_id <= 0)
                {
                    return BadRequest("Invalid product ID.");
                }
                var user_id = Convert.ToInt32(HttpContext.Items["Id"]);
                var data = await _cartServices.DecreaseQuantity(user_id, pro_id);
                return Ok(data);
            }

            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred: " + ex.Message);

            }
        }









    }
}
