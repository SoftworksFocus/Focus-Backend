namespace Focus.Domain.Entities
{
    public class Activity : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; } = true;
        public int UserId { get; set; }
        public User User { get; set; }  = null!;
        public int? GroupId { get; set; }
        public Group? Group { get; set; }
        public List<Media>? Media { get; set; }
    }
}
