using System.Security.Claims;
using Focus.Application.DTO.Group;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    private readonly IUserGroupService _userGroupService;
    private readonly IMediaUploadService _mediaUploadService;

    public GroupController(IGroupService groupService, IUserGroupService userGroupService,
        IMediaUploadService mediaUploadService)
    {
        _groupService = groupService;
        _userGroupService = userGroupService;
        _mediaUploadService = mediaUploadService;
    }

    // GET: api/<GroupController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetGroupDto>>> GetGroups(
        [FromQuery] string? groupnameFilter = null,
        [FromQuery] string? descriptionFilter = null
    )
    {
        try
        {
            var filterSpec = new GroupFilterSpecification(groupnameFilter, descriptionFilter);
            var groups = await _groupService.GetAllAsync(filterSpec);
            return Ok(groups);
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

    // GET api/<GroupController>/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetGroupDto>> GetById(int id)
    {
        try
        {
            var group = await _groupService.GetById(id);
            return Ok(group);
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

    // POST api/<GroupController>
    [HttpPost]
    public async Task<ActionResult<GetGroupDto>> Post([FromBody] CreateGroupDto groupDto)
    {
        try
        {
            var creatorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (creatorIdClaim == null)
            {
                return Unauthorized("Creator of the group not found.");
            }

            var creatorId = int.Parse(creatorIdClaim.Value);

            var createdGroup = await _groupService.CreateGroupAsync(groupDto, creatorId);

            return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error ocurred {ex.Message}");
        }
    }


    // PUT api/<GroupController>/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateGroupDto group)
    {
        try
        {
            var requesterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _groupService.UpdateAsync(id, group, requesterId);
            return Ok("Group updated successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
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

    // DELETE api/<GroupController>/5
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var requesterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _groupService.DeleteAsync(id, requesterId);
            return Ok("Group deleted successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
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

    // GET api/<GroupController>/5/members
    [HttpGet("{id:int}/members")]
    public async Task<ActionResult<IEnumerable<SummaryUserDto>>> GetMembers(int id)
    {
        try
        {
            var members = await _userGroupService.GetAllMembersFromGroup(id);
            return Ok(members);
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

    [HttpDelete("{groupId:int}/members/{userId:int}")]
    public async Task<IActionResult> RemoveMemberFromGroup([FromRoute] int groupId, [FromRoute] int userId)
    {
        try
        {
            var requesterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _userGroupService.RemoveUserFromGroupAsync(groupId, userId, requesterId);
            return Ok("Member removed successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // POST api/<GroupController>/5/profile-picture
    [HttpPost("{groupId:int}/profile-picture")]
    public async Task<ActionResult> UploadProfilePicture(int groupId, IFormFile file)
    {
        try
        {
            if (file.Length == 0)
            {
                return BadRequest("File is empty or not provided.");
            }

            var mediaUrl = await _mediaUploadService.UploadMediaAsync(file);
            await _groupService.UpdateProfilePicture(groupId, mediaUrl);
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