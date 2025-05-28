using Focus.Application.Services;
using Focus.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
        private readonly UserGroupService _userGroupService;

        public UserGroupController(UserGroupService userGroupService)
        {
            _userGroupService = userGroupService;
        }
        
        // GET: api/<User>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usersGroups = await _userGroupService.GetAll();
            
            return Ok(usersGroups);
        }

        // GET api/<User>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var  userGroup = await _userGroupService.GetById(id);
            
            return Ok(userGroup);
        }

        // POST api/<User>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserGroup userGroup)
        {
            await _userGroupService.Add(userGroup);
            
            return Ok();
        }

        // PUT api/<User>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UserGroup userGroup)
        {
            var existingUserGroup = await _userGroupService.GetById(id);
            await _userGroupService.Update(id, existingUserGroup);

            return Ok();
        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _userGroupService.Delete(id);
            
            return Ok();
        }
    }
}
