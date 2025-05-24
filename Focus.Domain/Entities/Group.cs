namespace Focus.Domain.Entities
{
    public class Group
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public List<UserGroup>? Members { get; set; }
        
    }
}
