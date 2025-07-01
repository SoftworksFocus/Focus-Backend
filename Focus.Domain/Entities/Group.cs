namespace Focus.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<UserGroup>? Members { get; set; }
    }
}
