using Focus.Application.DTO.Auth;
using Focus.Application.DTO.User;

namespace Focus.Application.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> AuthenticateAsync(LoginUserDto loginUserDto);
    Task<AuthResultDto> RefreshTokenAsync(string oldRefreshToken);
    Task LogoutAsync(string refreshToken);
}