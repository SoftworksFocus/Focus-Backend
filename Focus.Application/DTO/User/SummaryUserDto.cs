namespace Focus.Application.DTO.User;
using Domain.Entities;
public class SummaryUserDto : PlainUserDto
{
    public static SummaryUserDto FromUser(User user) =>
        new SummaryUserDto
        {
            ProfilePictureUrl = user.ProfilePictureUrl,
            Username = user.Username,
            Email = user.Email,
            Description = user.Description
        };
}