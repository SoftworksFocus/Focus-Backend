using Focus.Application.DTO.Activity;

namespace Focus.Application.DTO.Feed;

public class FeedResultDto
{
    public List<GetActivityDto> Activities { get; set; } = new List<GetActivityDto>();
    public DateTime? NextCursor { get; set; }
}