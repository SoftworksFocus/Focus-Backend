namespace Focus.Application.DTO.Group
{
    using Focus.Domain.Entities;

    public class CreateGroupDto : PlainGroupDto
    {
        public Group ToGroup() =>
            new Group
            {
                Name = Name,
                Description = Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
    }
}