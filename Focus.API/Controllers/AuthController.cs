using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService  _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
    {
        try
        {
            var token = await _authService.AuthenticateAsync(loginUserDto);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocorreu um erro inesperado. Tente novamente mais tarde.");
        }
    }
}