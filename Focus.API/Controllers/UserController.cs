using Microsoft.AspNetCore.Mvc;
using Focus.Application.Services;
using Focus.Application.DTO.User;


namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly UserGroupService _userGroupService;

        public UserController(UserService userService, UserGroupService userGroupService)
        {
            _userService = userService;
            _userGroupService = userGroupService;
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
                await _userGroupService.UserJoinGroup(userId, groupId);
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
                await _userGroupService.LeaveGroup(userId, groupId);
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