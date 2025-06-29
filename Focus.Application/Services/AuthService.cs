using Focus.Application.DTO.Auth;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Domain.Entities;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Focus.Application.Specifications;

namespace Focus.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _userTokenRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IUserTokenRepository userTokenRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
            _configuration = configuration;
        }

        public async Task<AuthResultDto> AuthenticateAsync(LoginUserDto loginUserDto)
        {
            var spec = new UserByEmailSpecification(loginUserDto.Email); //Todo: a spec for searching email | password
            var user = await _userRepository.GetFirstOrDefaultAsync(spec);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Invalid credentials!");
            }
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await CreateAndSaveRefreshTokenAsync(user);
            return new AuthResultDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResultDto> RefreshTokenAsync(string oldRefreshToken)
        {
            if (string.IsNullOrEmpty(oldRefreshToken))
            {
                throw new SecurityTokenException("Refresh token inválido.");
            }
            var oldTokenHash = HashingService.ComputeSha256Hash(oldRefreshToken);
            var userToken = await _userTokenRepository.GetByRefreshTokenHashAsync(oldTokenHash);
            if (userToken == null || userToken.IsRevoked || userToken.ExpiresAt <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Refresh token inválido ou expirado.");
            }
            userToken.IsRevoked = true;
            await _userTokenRepository.UpdateAsync(userToken);
            var newAccessToken = GenerateJwtToken(userToken.User);
            var newRefreshToken = await CreateAndSaveRefreshTokenAsync(userToken.User);
            return new AuthResultDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        
        public async Task LogoutAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return;
            var tokenHash = HashingService.ComputeSha256Hash(refreshToken);
            var userToken = await _userTokenRepository.GetByRefreshTokenHashAsync(tokenHash);
            if (userToken != null && !userToken.IsRevoked)
            {
                userToken.IsRevoked = true;
                await _userTokenRepository.UpdateAsync(userToken);
            }
        }
        
        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("The configuration 'Jwt:Key' has not been found. Verify the configuration file (appsettings.json, secrets.json)."); //Todo: translate
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] //Todo: collection expression
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<string> CreateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshTokenHash = HashingService.ComputeSha256Hash(refreshToken);
            var userToken = new UserToken
            {
                UserId = user.Id,
                RefreshTokenHash = refreshTokenHash,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userTokenRepository.AddAsync(userToken);
            return refreshToken;
        }
    }
}