namespace Focus.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Description { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresAt { get; set; } 

        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; } 
        
        public string? PendingNewEmail { get; set; }

        public IEnumerable<Group>? OwnedGroups { get; set; }
        public List<UserGroup>? Groups { get; set; } 
        public List<Activity>? Activities { get; set; }
        public List<UserToken>? UserTokens { get; set; }
    }
}
