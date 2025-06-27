namespace Focus.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
        public string? Description { get; set; }
        public IEnumerable<Group>? OwnedGroups { get; set; }
        public List<UserGroup>? Groups { get; set; } 
        public List<Activity>? Activities { get; set; }
    }
}
