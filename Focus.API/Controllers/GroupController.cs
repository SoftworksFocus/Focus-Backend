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

        GroupController(GroupService groupService)
        {
            _groupService =  groupService;
        }
        
        // GET: api/<GroupController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            var groups =  await _groupService.GetAll();

            return Ok(groups);
        }

        // GET api/<GroupController>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var group =  await _groupService.GetById(id);

            return Ok(group);
        }

        // POST api/<GroupController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Group group)
        {
            await _groupService.Add(group);

            return Ok();
        }

        // PUT api/<GroupController>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Group group)
        {
            await _groupService.Update(id, group);
            
            return Ok();
        }

        // DELETE api/<GroupController>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _groupService.Delete(id);
            
            return Ok();
        }
    }
}
