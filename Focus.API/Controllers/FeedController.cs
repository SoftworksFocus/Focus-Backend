using Focus.Application.DTO.Activity;
using Focus.Application.Services;
using Focus.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Focus.API.Controllers;


    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedController : ControllerBase
    {

        private readonly IFeedService _feedService;
        
        public FeedController(IFeedService feedService)
        {
            _feedService = feedService;
        }
        
        // GET: api/<FeedController>
        [HttpGet("{userId:int}")]
        public async Task<ActionResult<List<GetActivityDto>>> Get([FromRoute] int userId, [FromQuery] DateTime? cursor, [FromQuery] int pageSize = 10)
        {
            try
            {
                var activities = await _feedService.GetFeedForUserAsync(userId, cursor, pageSize);
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