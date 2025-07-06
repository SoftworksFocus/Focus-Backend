namespace Focus.Application.DTO.Media;

public class PlainMediaDto
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public int DisplayOrder { get; set; }
    
    public static PlainMediaDto FromMedia(Domain.Entities.Media media)
    {
        return new PlainMediaDto
        {
            Id = media.Id,
            Url = media.Url,
            DisplayOrder = media.DisplayOrder
        };
    }
}