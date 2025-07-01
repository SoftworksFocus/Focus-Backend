using System.Security.Claims;
using Focus.Application.DTO.User;
using Focus.Application.Services;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Focus.Infra.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Focus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    
    public AuthController(
        IAuthService authService, 
        IUserRepository userRepository, 
        IEmailService emailService)
    {
        _authService = authService;
        _userRepository = userRepository;
        _emailService = emailService;
    }
    
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var authResult = await _authService.AuthenticateAsync(loginUserDto);
            SetRefreshTokenInCookie(authResult.RefreshToken);
            return Ok(new { accessToken = authResult.AccessToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    [HttpPost("refresh-token")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        var oldRefreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(oldRefreshToken))
        {
            return Unauthorized(new { message = "Refresh token not found." });
        }
        try
        {
            var authResult = await _authService.RefreshTokenAsync(oldRefreshToken);
            SetRefreshTokenInCookie(authResult.RefreshToken);
            return Ok(new { accessToken = authResult.AccessToken });
        }
        catch (SecurityTokenException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }
        Response.Cookies.Delete("refreshToken");
        return Ok(new { message = "Logout successful." });
    }
    
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var spec = new UserByEmailSpecification(forgotPasswordDto.Email);
        var user = await _userRepository.GetFirstOrDefaultAsync(spec);
        if (user == null)
        {
            return Ok(new { message = "If the provided email is registered, a password reset link will be sent." });
        }
        var token = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));
        user.PasswordResetToken = HashingService.ComputeSha256Hash(token);
        user.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);
        await _userRepository.UpdateAsync(user.Id, user);
        await _emailService.SendPasswordResetEmail(user.Email, token);
        return Ok(new { message = "If the provided email is registered, a password reset link will be sent." });
    }
    
    [HttpPost("reset-password")]
    [AllowAnonymous] //Todo: verify a logic to deny malicious activity (using the endpoint without being a user)
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var spec = new UserByEmailSpecification(resetPasswordDto.Email);
        var user = await _userRepository.GetFirstOrDefaultAsync(spec);
        var hashedTokenFromRequest = HashingService.ComputeSha256Hash(resetPasswordDto.Token);
        if (user == null || user.PasswordResetToken != hashedTokenFromRequest || user.PasswordResetTokenExpiresAt <= DateTime.UtcNow)
        {
            return BadRequest(new { message = "Invalid or expired password reset link." });
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiresAt = null;
        await _userRepository.UpdateAsync(user.Id, user);
        return Ok(new { message = "Your password has been reset successfully." });
    }

    private void SetRefreshTokenInCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            Secure = true, 
            SameSite = SameSiteMode.Strict
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}