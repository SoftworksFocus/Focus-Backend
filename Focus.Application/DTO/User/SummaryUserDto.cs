namespace Focus.Application.DTO.User;
using Domain.Entities;
public class SumaryUserDto : PlainUserDto
{
    public static SumaryUserDto FromUser(User user) =>
        new SumaryUserDto
        {
            Username = user.Username,
            Email = user.Email,
            Description = user.Description
        };
}