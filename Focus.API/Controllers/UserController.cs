using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Focus.Application.Services.Interfaces;
using Focus.Application.DTO.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Focus.Application.Services;
using Focus.Infra.Repositories.Interfaces;
using Focus.Application.Specifications;
using Focus.Domain.Entities;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupService _userGroupService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public UserController(
            IUserService userService, 
            IUserGroupService userGroupService, 
            ITokenService tokenService, 
            IUserRepository userRepository, 
            IEmailService emailService)
        {
            _userService = userService;
            _userGroupService = userGroupService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        // GET: api/<User>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var returnUsers = await _userService.GetAllAsync();
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
        [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] CreateUserDto userDto)
        {
            try
            {
                await _userService.Add(userDto);
                return Ok(new { message = "User created successfully. Please check your email to verify your account."});
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

        // PUT api/<User>/5
        [HttpPut("{id:int}")]
        [Authorize]
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
        [Authorize]
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

        [HttpGet("groups/{userId:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllGroupsFromUser([FromRoute] int userId)
        {
            try
            {
                var returnUsersGroups = await _userGroupService.GetAllGroupsFromUser(userId);
                return Ok(returnUsersGroups);
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

        [HttpPost("join/{groupId:int}/{userId:int}")]
        [Authorize]
        public async Task<IActionResult> JoinGroup([FromRoute] int groupId, [FromRoute] int userId)
        {
            try
            {
                await _userGroupService.AddUserToGroupAsync(userId, groupId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
        
        [HttpPut("toggle-admin/{groupId:int}/{userId:int}")]
        [Authorize]
        public async Task<IActionResult> ToggleAdminRole([FromRoute] int groupId, [FromRoute] int userId)
        {
            try
            {
                await _userGroupService.ToggleRoleAdmin(userId, groupId);
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

        [HttpDelete("leave/{groupId:int}/{userId:int}")]
        [Authorize]
        public async Task<IActionResult> LeaveGroup([FromRoute] int groupId, [FromRoute] int userId)
        {
            try
            {
                await _userGroupService.RemoveUserFromGroupAsync(userId, groupId);
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
        
        [HttpPost("change-email-request")]
        [Authorize]
        public async Task<IActionResult> RequestEmailChange([FromBody] ChangeEmailRequestDto requestDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetByIdAsync(userId);
    
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

            user.PendingNewEmail = requestDto.NewEmail;
            user.EmailVerificationToken = HashingService.ComputeSha256Hash(token);
            user.EmailVerificationTokenExpiresAt = DateTime.UtcNow.AddHours(1);

            await _userRepository.UpdateAsync(user.Id, user);
    
            await _emailService.SendConfirmNewEmailAsync(requestDto.NewEmail, token);

            return Ok(new { message = $"A confirmation link has been sent to {requestDto.NewEmail}." });
        }
        
        [HttpGet("confirm-email-change")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChange([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required." });
            }

            var hashedToken = HashingService.ComputeSha256Hash(token);
            var user = await _userRepository.FindByEmailVerificationTokenAsync(hashedToken);

            if (user == null || user.EmailVerificationTokenExpiresAt <= DateTime.UtcNow)
            {
                return BadRequest(new { message = "Invalid or expired confirmation link." });
            }

            user.Email = user.PendingNewEmail;
            user.PendingNewEmail = null;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiresAt = null;

            await _userRepository.UpdateAsync(user.Id, user);

            return Ok("Your email address has been successfully updated.");
        }
    }
}