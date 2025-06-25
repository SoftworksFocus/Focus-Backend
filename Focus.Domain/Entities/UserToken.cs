namespace Focus.Domain.Entities;

public class UserToken
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string RefreshTokenHash { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
}