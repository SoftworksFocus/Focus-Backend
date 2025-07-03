
using Focus.Application.DTO.Activity;
using Focus.Application.DTO.Media;
using Focus.Application.Services;
using Focus.Application.Services.Interfaces;
using Focus.Application.Specifications;
using Microsoft.AspNetCore.Mvc;
using Focus.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Focus.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;
        private readonly IMediaUploadService _mediaUploadService;

        public ActivityController(IActivityService activityService, IMediaUploadService mediaUploadService)
        {
            _activityService = activityService;
            _mediaUploadService = mediaUploadService;
        }

        // GET: api/Activity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetActivityDto>>> GetActivities(
            [FromQuery] string? ownerUsernameFilter = null,
            [FromQuery] int? groupId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            try
            {
                var spec = new ActivityFilterSpecification(ownerUsernameFilter, groupId);
                var activities = await _activityService.GetAllAsync(spec, pageNumber, pageSize);
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
        
        // POST: api/Activity/{activityId:int}/upload-media
        [HttpPost("{activityId:int}/upload-media")]
        public async Task<IActionResult> UploadMedia([FromRoute] int activityId, [FromForm] UploadMediaDto uploadDto)
        {
            try
            {
                // var caption = "debug";
                if (uploadDto.File.Length == 0)
                {
                    return BadRequest("File is required.");
                }
                var mediaUrl = await _mediaUploadService.UploadMediaAsync(uploadDto.File);
                if (string.IsNullOrEmpty(mediaUrl))
                {
                    return StatusCode(500, "Failed to upload media.");
                }

                await _activityService.UpdateMedia(activityId, mediaUrl, uploadDto.Caption);
                return CreatedAtAction(nameof(UploadMedia), 
                    new { activityId }, 
                    new { MediaUrl = mediaUrl, Caption = uploadDto.Caption });
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
