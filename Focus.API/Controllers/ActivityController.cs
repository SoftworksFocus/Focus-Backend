
using Focus.Application.DTO.Activity;
using Focus.Application.Services;
using Focus.Application.Specifications;
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
        public async Task<ActionResult<IEnumerable<GetActivityDto>>> GetActivities(
            [FromQuery] string? ownerUsernameFilter = null,
            [FromQuery] int? groupId = null
            )
        {
            try
            {
                var spec = new ActivityFilterSpecification(ownerUsernameFilter, groupId);
                var activities = await _activityService.GetAllAsync(spec);
                return Ok(activities);
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

        // GET: api/Activity/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            try
            {
                var activity = await _activityService.GetById(id);
                return Ok(activity);
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

        // PUT: api/Activity/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateActivityDto activity)
        {
            try
            {
                await _activityService.Update(id, activity);
                return NoContent();
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

        // POST: api/Activity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Create(CreateActivityDto activity)
        {
            try
            {
                await _activityService.Add(activity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Activity/5
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {
            try
            {
                await _activityService.Delete(id);
                return NoContent();
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
