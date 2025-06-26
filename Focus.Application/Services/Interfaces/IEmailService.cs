namespace Focus.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmail(string toEmail, string token);
    Task SendPasswordResetEmail(string toEmail, string token);
}