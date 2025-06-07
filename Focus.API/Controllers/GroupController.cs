using Focus.Application.DTO.Group;
using Focus.Application.Services;
using Focus.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _groupService;

        public GroupController(GroupService groupService)
        {
            _groupService =  groupService;
        }
        
        // GET: api/<GroupController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetGroupDto>>> GetGroups()
        {
            try
            {
                var groups = await _groupService.GetAll();
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
            await _groupService.Add(group);
            return Ok();
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id:int}")]
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
    }
}
