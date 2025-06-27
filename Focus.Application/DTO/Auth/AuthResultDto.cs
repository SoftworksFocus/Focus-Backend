namespace Focus.Application.DTO.Auth;

    public class AuthResultDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }