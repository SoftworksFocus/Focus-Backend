using Microsoft.AspNetCore.Mvc;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;

namespace Focus.API.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserGroupService _userGroupService;
    private readonly ITokenService _tokenService;
    private readonly IMediaUploadService _mediaUploadService;

    public UserController
    (
        IUserService userService, 
        IUserGroupService userGroupService, 
        ITokenService tokenService,
        IMediaUploadService mediaUploadService
    )

    {
        _userService = userService;
        _userGroupService = userGroupService;
        _tokenService = tokenService;
        _mediaUploadService = mediaUploadService;
    }

    // GET: api/<User>
    [HttpGet]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? usernameFilter = null,
        [FromQuery] string? emailFilter = null
    )
    {
        try
        {
            var spec = new UserFilterSpecification(usernameFilter, emailFilter);
            var returnUsers = await _userService.GetAllAsync(spec);
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

    // GET api/<User>/groups/1
    [HttpGet("groups/{userId:int}")]
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

    // POST api/<User>/join//1/2
    [HttpPost("join/{groupId:int}/{userId:int}")]
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

    // PUT api/<User>/toggle-admin/1/2
    [HttpPut("toggle-admin/{groupId:int}/{userId:int}")]
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

    // DELETE api/<User>/leave/{groupId:int}/{userId:int}
    [HttpDelete("leave/{groupId:int}/{userId:int}")]
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
    
    // POST api/<User>/{userId}/profile-picture
    [HttpPost("{userId:int}/profile-picture")]
    public async Task<IActionResult> UploadProfilePicture([FromRoute] int userId, IFormFile file)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            var mediaUrl = await _mediaUploadService.UploadMediaAsync(file);
            await _userService.UpdateProfilePicture(userId, mediaUrl);
            return Ok(new { MediaUrl = mediaUrl });
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