using Focus.Application.DTO.Activity;
using Focus.Application.Services;
using Microsoft.AspNetCore.Mvc;
namespace Focus.API.Controllers;


    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {

        private readonly FeedService _feedService;
        
        public FeedController(FeedService feedService)
        {
            _feedService = feedService;
        }
        
        // GET: api/<FeedController>
        [HttpGet("/{userId:int}")]
        public async Task<ActionResult<List<GetActivityDto>>> Get([FromRoute] int userId)
        {
            try
            {
                var activities = await _feedService.GetFeedForUserAsync(userId);
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
    }
