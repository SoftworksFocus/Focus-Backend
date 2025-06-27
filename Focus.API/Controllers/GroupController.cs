using Focus.Application.DTO.Group;
using Focus.Application.DTO.User;
using Focus.Application.Services;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Focus.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IUserGroupService _userGroupService;

        public GroupController(IGroupService groupService, IUserGroupService userGroupService)
        {
            _groupService = groupService;
            _userGroupService = userGroupService;
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
        [Authorize]
        public async Task<ActionResult> Post([FromBody] CreateGroupDto group)
        {
            await _groupService.Add(group);
            return Ok();
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateGroupDto group)
        {
            try
            {
                await _groupService.Update(id, group);
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

        // DELETE api/<GroupController>/5
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _groupService.Delete(id);
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

        // GET api/<GroupController>/5/members
        [HttpGet("{id:int}/members")]
        [Authorize]
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
    }
}
