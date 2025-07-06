namespace Focus.Application.DTO.User;

public class MemberDto : SummaryUserDto
{
    public bool IsAdmin { get; set; }
    
    public static MemberDto FromUser(Domain.Entities.User user, bool isAdmin)
    {
        return new MemberDto
        {
            ProfilePictureUrl = user.ProfilePictureUrl,
            Username = user.Username,
            Email = user.Email,
            Description = user.Description,
            IsAdmin = isAdmin
        };
    }
    
}