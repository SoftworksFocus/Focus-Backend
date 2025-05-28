using Focus.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserGroupController : ControllerBase
    {
       // private readonly UserGroupService _userGroupService;
        
        // GET: api/<UserGroupController>
        [HttpGet]
        // public async Task<ActionResult<IEnumerable<UserGroup>>> GetUserGroups()
        // {
        //     
        // }

        // GET api/<UserGroupController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserGroupController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserGroupController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserGroupController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
