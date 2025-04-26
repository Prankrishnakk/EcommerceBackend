using EcommerceBackend.ApiResponse;
using EcommerceBackend.Dto;
using EcommerceBackend.Services.AutheticationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authServices;
        public AuthenticationController(IAuthenticationService authServices)
        {
            _authServices = authServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegisterationDto newUser)
        {
            try
            {
                bool isdone = await _authServices.Register(newUser);
                if (!isdone)
                {
                    return BadRequest(new ApiResponse<string>(false, "User already exists", "[]", null));
                }

                return Ok(new ApiResponse<string>(true, "User registerd succesfully", "[done]", null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm]UserLoginDto login)
        {
            try
            {
                var res = await _authServices.Login(login);

                if (res.Error == "Not Found")
                {
                    return NotFound("Email is not verified");
                }

                if (res.Error == "invalid password")
                {
                    return BadRequest(res.Error);
                }

                if (res.Error == "User Blocked")
                {
                    return StatusCode(403, "User is blocked by admin!");
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
