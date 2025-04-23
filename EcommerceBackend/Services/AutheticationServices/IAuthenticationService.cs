using EcommerceBackend.Dto;

namespace EcommerceBackend.Services.AutheticationServices
{
    public interface IAuthenticationService
    {
        Task<bool> Register(UserRegisterationDto userRegisterationdto);
        Task<UserResponseDto> Login(UserLoginDto userLoginDto);
    }
}
