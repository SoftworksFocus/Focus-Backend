using Focus.Application.DTO.User;

namespace Focus.Application.Services.Interfaces;

public interface IAuthService
{
    public Task<string> AuthenticateAsync(LoginUserDto loginUserDto);
    public Task LogoutAsync(int userId);
}