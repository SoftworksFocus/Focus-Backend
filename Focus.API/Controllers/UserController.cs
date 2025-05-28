using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Focus.Application.Services;
using Focus.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            var users = await _userService.GetAll();
            
            return Ok(users);
        }

        // GET api/<User>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var  user = await _userService.GetById(id);
            
            return Ok(user);
        }

        // POST api/<User>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            await _userService.Add(user);
            
            return Ok();
        }

        // PUT api/<User>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] User user)
        {
            var existingUser = await _userService.GetById(id);
            await _userService.Update(id, user);

            return Ok();
        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _userService.Delete(id);
            return Ok();
        }
    }
}
