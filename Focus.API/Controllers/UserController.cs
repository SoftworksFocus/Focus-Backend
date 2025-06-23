using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Focus.Application.Services;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Focus.Infra.Repositories;


namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserRepository _userRepository;
        private readonly TokenService _tokenService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        
        // GET: api/<User>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var returnUsers = await _userService.GetAll();
                return Ok(returnUsers);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<User>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/<User>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserDto userDto)
        {
            try
            {
                await _userService.Add(userDto);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

[HttpPost("refresh-token")]
public async Task<IActionResult> RefreshToken()
{
    var refreshToken = Request.Cookies["refreshToken"];

    if (string.IsNullOrEmpty(refreshToken))
    {
        return Unauthorized(new { message = "Refresh token não encontrado." });
    }


    var user = await _userRepository.GetUserByRefreshTokenAsync(refreshToken);


    if (user == null)
    {
        return Unauthorized(new { message = "Refresh token inválido." });
    }

    if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
        return Unauthorized(new { message = "Refresh token expirado. Por favor, faça login novamente." });
    }


    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };
    var newAccessToken = _tokenService.GenerateAccessToken(claims);


    var newRefreshToken = _tokenService.GenerateRefreshToken();
    user.RefreshToken = newRefreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

    await _userRepository.UpdateAsync(user.Id, user);


    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,
        Expires = user.RefreshTokenExpiryTime,
        Secure = true,
        SameSite = SameSiteMode.Strict
    };
    Response.Cookies.Append("refreshToken", newRefreshToken, cookieOptions);


    return Ok(new { accessToken = newAccessToken });
}

        // PUT api/<User>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            try
            {
                await _userService.Update(id, userDto);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<User>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _userService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
