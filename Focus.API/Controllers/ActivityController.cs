
using Focus.Application.DTO.Activity;
using Focus.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Focus.Domain.Entities;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly ActivityService _activityService;

        public ActivityController(ActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: api/Activity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetActivityDto>>> GetActivities()
        {
            var activities =  await _activityService.GetAll();
            return Ok(activities);
        }

        // GET: api/Activity/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetActivityDto>> GetById([FromRoute]int id)
        {
            var activity = await _activityService.GetById(id);

            return Ok(activity);
        }

        // PUT: api/Activity/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateActivityDto activity)
        {
            await _activityService.Update(id, activity);

            return NoContent();
        }

        // POST: api/Activity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Activity>> Create(CreateActivityDto activity)
        {
            await _activityService.Add(activity);

            return Ok();
        }

        // DELETE: api/Activity/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            await _activityService.Delete(id);
            return NoContent();
        }
        
    }
}
