using Focus.Application.Services;
using Focus.Domain.Entities;
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
        [HttpGet("{userId:int}/{groupId:int}")]
        public async Task<IActionResult> Get([FromRoute] int userId, [FromRoute] int groupId)
        {
            var  userGroup = await _userGroupService.GetById(userId, groupId);
            
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
        [HttpPut("{userId:int}/{groupId:int}")]
        public async Task<IActionResult> Update([FromRoute] int userId, [FromRoute] int groupId, [FromBody] UserGroup newUserGroup)
        {
            await _userGroupService.Update(userId, groupId, newUserGroup);

            return Ok();
        }

        // DELETE api/<User>/5
        [HttpDelete("{userId:int}/{groupId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int userId, [FromRoute] int groupId)
        {
            await _userGroupService.Delete(userId, groupId);
            
            return Ok();
        }
    }
}
