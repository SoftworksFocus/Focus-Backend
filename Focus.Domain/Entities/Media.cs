namespace Focus.Domain.Entities;

public class Media : BaseEntity
{
    public string Url { get; set; } = null!;
    public string? Caption { get; set; }
    public int DisplayOrder { get; set; }
    public int ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
}
