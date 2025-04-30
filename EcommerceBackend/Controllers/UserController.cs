using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _services;
        public UserController(IUserServices services)
        {
            _services = services;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("gett-all-users")]
        public async Task<IActionResult> getUsers()
        {
            try
            {
                var users = await _services.ListUsers();

                if (users == null)
                {
                    return NotFound(new ApiResponse<string>(false, "no users in the list", "", null));
                }

                return Ok(new ApiResponse<IEnumerable<UserViewDto>>(true, "done", users, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [Authorize(Roles = "Admin")]
        [HttpGet("userbyId/{id}")]
        public async Task<IActionResult> userByid(int id)
        {
            try
            {
                var user = await _services.GetUser(id);

                if (user == null)
                {
                    return NotFound(new ApiResponse<string>(false, "no match users in the list", "", null));
                }

                return Ok(new ApiResponse<UserViewDto>(true, "done", user, null));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("block/unblock{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> block_unb(int id)
        {
            try
            {
                var res = await _services.BlockUnblockUser(id);

                return Ok(new ApiResponse<BlockUnblockDto>(true, "updated", res, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}

