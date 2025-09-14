using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SharedLibrary.SharedKernel.Enum;
using System.Collections.Concurrent;
using System.Threading.Tasks.Sources;

namespace SharedLibrary.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;
        private static readonly ConcurrentDictionary<string, string> _templateCache = new();
        private readonly string _templateBasePath;

        public EmailService(IOptions<EmailSettings> emailSettings, IConfiguration configuration)
        {
            _emailSettings = emailSettings?.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _templateBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "Template");
            _configuration = configuration;
        }
        public async Task<bool> SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            try
            {
                await SendEmailAsync(toEmail, EmailType.VerifyOPTCode, null, verificationCode);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LOI NE: {ex}");
                return false;
            }
        }
        private IConfigurationSection GetEmailSettings()
        {
            return _configuration.GetSection("EMAIL");
        }
        public async Task SendForgotPasswordAsync(string toEmail, string resetToken)
        {
            await SendEmailAsync(toEmail, EmailType.ForgotPassword, resetToken);
        }
        private async Task SendEmailAsync(string toEmail, EmailType emailType, string? token = null, string? plainPassword = null)
        {


            var emailBody = await GenerateEmailBodyAsync(toEmail, emailType, token, plainPassword);
            var emailMessage = BuildEmailMessage(toEmail, emailBody, emailType);
            await SendEmailViaSmtpAsync(emailMessage);

        }

        private async Task<string> GenerateEmailBodyAsync(string email, EmailType emailType, string? token, string? plainPassword)
        {
            var templateFileName = GetTemplateFileName(emailType);
            var htmlTemplate = await LoadEmailTemplateAsync(templateFileName);

            return ReplaceTemplatePlaceholders(htmlTemplate, email, token, plainPassword, emailType);
        }

        private static string GetTemplateFileName(EmailType emailType)
        {
            return emailType switch
            {
                EmailType.VerifyOPTCode => "EmailVerificationCode.html",
                EmailType.ForgotPassword => "ForgotPassword.html",
                _ => "EmailVerificationCode.html" // Default template
            };
        }

        private async Task<string> LoadEmailTemplateAsync(string templateFileName)
        {
            var cacheKey = templateFileName;
            var templatePath = Path.Combine(_templateBasePath, templateFileName);
            var template = await File.ReadAllTextAsync(templatePath);
            _templateCache.TryAdd(cacheKey, template);
            return template;

        }

        private static string ReplaceTemplatePlaceholders(string template, string email, string? token, string? plainPassword, EmailType emailType)
        {
            var result = template
                .Replace("{{email}}", email)
                .Replace("{{username}}", email);

            switch (emailType)
            {
                case EmailType.VerifyOPTCode:
                    if (!string.IsNullOrEmpty(plainPassword))
                    {
                        result = result.Replace("{{verification_code}}", plainPassword);
                    }
                    break;

                case EmailType.ForgotPassword:
                    var resetLink = GenerateResetPasswordLink(token);
                    result = result.Replace("{{confirm_link}}", resetLink);
                    result = result.Replace("{{reset_link}}", resetLink);
                    break;
            }

            return result;
        }

        private static string GenerateResetPasswordLink(string? token)
        {
            if (string.IsNullOrEmpty(token))
                return "#";

            // This should be configured based on your frontend URL
            return $"https://your-frontend-domain.com/reset-password?token={token}";
        }

        private MimeMessage BuildEmailMessage(string toEmail, string body, EmailType emailType)
        {
            var subject = GetEmailSubject(emailType);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SENDER_NAME, _emailSettings.SENDER_EMAIL));
            message.To.Add(new MailboxAddress(string.Empty, toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private static string GetEmailSubject(EmailType emailType)
        {
            return emailType switch
            {
                EmailType.VerifyOPTCode => "Mã xác thực email DriveMate",
                EmailType.ForgotPassword => "Khôi phục mật khẩu DriveMate",
                _ => "Thông báo từ DriveMate"
            };
        }

        private async Task SendEmailViaSmtpAsync(MimeMessage message)
        {
            var emailSettings = GetEmailSettings();
            using var client = new SmtpClient();


            await client.ConnectAsync(_emailSettings.SMTP_SERVER, _emailSettings.SMTP_PORT, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SENDER_EMAIL, _emailSettings.SENDER_PASSWORD);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

    }
}
