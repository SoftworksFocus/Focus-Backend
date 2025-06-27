using Focus.Application.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Focus.Infra.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendVerificationEmail(string toEmail, string token)
    {
        var verificationLink = $"{_configuration["FrontendUrls:EmailVerification"]}{token}";
        
        var emailBody = $"<h1>Welcome to Focus!</h1>" +
                        $"<p>Please click the link below to verify your email address:</p>" +
                        $"<a href='{verificationLink}'>Verify Email</a>";

        await SendEmailAsync(toEmail, "Confirm your Email - Focus App", emailBody);
    }

    public async Task SendPasswordResetEmail(string toEmail, string token)
    {
        var resetLink = $"{_configuration["FrontendUrls:PasswordReset"]}{token}";
        
        var emailBody = $"<h1>Password Reset</h1>" +
                        $"<p>You requested a password reset. Please click the link below to create a new one:</p>" +
                        $"<a href='{resetLink}'>Reset Password</a>";

        await SendEmailAsync(toEmail, "Password Reset Request - Focus App", emailBody);
    }
    
    public async Task SendConfirmNewEmailAsync(string toEmail, string token)
    {
        //Todo: Verify the link from the frontend
        var confirmationLink = $"{_configuration["FrontendUrls:EmailChangeConfirmation"]}{token}";
        
        var emailBody = $"<h1>Confirm your new Email Address</h1>" +
                        $"<p>To finalize your email change, please click the link below:</p>" +
                        $"<a href='{confirmationLink}'>Confirm New Email</a>";

        await SendEmailAsync(toEmail, "Confirm your New Email - Focus App", emailBody);
    }

    private async Task SendEmailAsync(string to, string subject, string htmlContent)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(
            _configuration["EmailSettings:SenderName"],
            _configuration["EmailSettings:SenderEmail"])
        );
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html.ToString(), htmlContent);

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _configuration["EmailSettings:SmtpServer"],
            int.Parse(_configuration["EmailSettings:Port"]),
            SecureSocketOptions.StartTls
        );

        await smtp.AuthenticateAsync(
            _configuration["EmailSettings:Username"],
            _configuration["EmailSettings:Password"]
        );

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}