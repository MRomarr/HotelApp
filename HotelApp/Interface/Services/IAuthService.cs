using HotelApp.DTOs.Auth;

namespace HotelApp.Interface.Services
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthDto> LoginAsync(LoginDto loginDto);
        Task<string> AddRoleAsync(AddRoleDto model);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto model);
        Task<bool> ResetPasswordAsync(string userId, string token, string NewPassword);
    }
}
