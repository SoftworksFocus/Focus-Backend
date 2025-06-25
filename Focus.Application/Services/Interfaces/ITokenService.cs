using System.Security.Claims;

namespace Focus.Application.Services.Interfaces;

public interface ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
}