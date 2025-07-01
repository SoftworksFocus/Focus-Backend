using Focus.Application.DTO.Group;
using Focus.Application.DTO.User;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Focus.Application.DTO;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IUserGroupService _userGroupService;
        private readonly IMediaUploadService _mediaUploadService;

        public GroupController(IGroupService groupService, IUserGroupService userGroupService, IMediaUploadService mediaUploadService)
        {
            _groupService = groupService;
            _userGroupService = userGroupService;
            _mediaUploadService = mediaUploadService;
        }

        // GET: api/<GroupController>
        [HttpGet]
        public async Task<IActionResult> GetGroups(
            [FromQuery] string? groupnameFilter = null,
            [FromQuery] string? descriptionFilter = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var filterSpec = new GroupFilterSpecification(groupnameFilter, descriptionFilter);
                var groups = await _groupService.GetAllAsync(filterSpec, pageNumber, pageSize);
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
        public async Task<ActionResult> Post([FromBody] CreateGroupDto group)
        {
            try
            {
                var creatorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (creatorIdClaim == null)
                {
                    return Unauthorized("Creator of the group not found.");
                }

                var creatorId = int.Parse(creatorIdClaim.Value);
                var createdGroup = await _groupService.CreateGroupAsync(group, creatorId);
                return CreatedAtAction(nameof(GetById), new { id = createdGroup.Id }, createdGroup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error occurred: {ex.Message}");
            }
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateGroupDto group)
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
