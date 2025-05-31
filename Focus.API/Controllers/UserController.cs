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

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        // GET: api/<User>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var returnUsers = await _userService.GetAll();
            return Ok(returnUsers);
        }

        // GET api/<User>/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        // POST api/<User>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserDto userDto)
        {
            await _userService.Add(userDto);
            
            return Ok();
        }

        // PUT api/<User>/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto userDto)
        {
            await _userService.Update(id, userDto);
            return Ok();
        }

        // DELETE api/<User>/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}
