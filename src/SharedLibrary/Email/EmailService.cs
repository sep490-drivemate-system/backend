using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SharedLibrary.SharedKernel.Enum;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace SharedLibrary.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, string> _templateCache = new();
        private readonly string _templateBasePath;

        public EmailService( IConfiguration configuration )
        {
            _templateBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "Template");
            _configuration = configuration;
        }
        public async Task<bool> SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            try
            {
                await Task.Run(() => SendEmail(toEmail, EmailType.VerifyOPTCode, null, verificationCode));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LOI NE: {ex}");
                return false;
            }
        }
        public async Task SendForgotPasswordAsync(string toEmail, string resetToken)
        {
            await Task.Run(() => SendEmail(toEmail, EmailType.ForgotPassword, resetToken));
        }
        public async Task<bool> SendInstructorWelcomingAsync(string toEmail, DateOnly verificationExpirationDate)
        {
            var htmlTemplate = LoadEmailTemplate(GetTemplateFileName(EmailType.InstructorRegistration));

            string mesageBody = htmlTemplate.Replace("{{username}}", toEmail)
                .Replace("{{expiry_date}}", verificationExpirationDate.ToString("dd/MM/yyyy"))
                .Replace("{{login_url}}", _configuration["FRONTEND:LOGIN"]);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EMAIL:SENDER_NAME"], _configuration["EMAIL:SENDER_EMAIL"]));
            message.To.Add(new MailboxAddress(string.Empty, toEmail));
            message.Subject = GetEmailSubject(EmailType.InstructorRegistration);
            var bodyBuilder = new BodyBuilder { HtmlBody = mesageBody};
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                await Task.Run(() => SendEmailViaSmtp(message));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }


        #region "Behind the scene"
        private void SendEmail(string toEmail, EmailType emailType, string? token = null, string? plainPassword = null)
        {
            var emailBody = GenerateEmailBody(toEmail, emailType, token, plainPassword);
            var emailMessage = BuildEmailMessage(toEmail, emailBody, emailType);
            SendEmailViaSmtp(emailMessage);
        }

        private string GenerateEmailBody(string email, EmailType emailType, string? token, string? plainPassword)
        {
            var templateFileName = GetTemplateFileName(emailType);
            var htmlTemplate = LoadEmailTemplate(templateFileName);

            return ReplaceTemplatePlaceholders(htmlTemplate, email, token, plainPassword, emailType);
        }

        private static string GetTemplateFileName(EmailType emailType)
        {
            return emailType switch
            {
                EmailType.VerifyOPTCode => "EmailVerificationCode.html",
                EmailType.ForgotPassword => "ForgotPassword.html",
                EmailType.InstructorRegistration => "InstructorRegistration.html",
                EmailType.InstructorReschedule => "SessionRescheduledInstructor.html",
                EmailType.DriverReschedule => "SessionRescheduledDriver.html",
                _ => "EmailVerificationCode.html" // Default template
            };
        }

        private string LoadEmailTemplate(string templateFileName)
        {
            var cacheKey = templateFileName;
            
            // Kiểm tra cache trước
            if (_templateCache.TryGetValue(cacheKey, out var cachedTemplate))
            {
                return cachedTemplate;
            }
            
            var templatePath = Path.Combine(_templateBasePath, templateFileName);
            var template = File.ReadAllText(templatePath);
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
            message.From.Add(new MailboxAddress(_configuration["EMAIL:SENDER_NAME"], _configuration["EMAIL:SENDER_EMAIL"]));
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
                EmailType.InstructorRegistration => "Chào mừng đến với DriveMate",
                _ => "Thông báo từ DriveMate"
            };
        }

        private void SendEmailViaSmtp(MimeMessage message)
        {
            using var client = new SmtpClient();
            // client.ServerCertificateValidationCallback = (s, c, h, e) => true;
            client.Connect(_configuration["EMAIL:SMTP_SERVER"], int.Parse(_configuration["EMAIL:SMTP_PORT"]), SecureSocketOptions.StartTls);
            client.Authenticate(_configuration["EMAIL:SENDER_EMAIL"], _configuration["EMAIL:SENDER_PASSWORD"]);
            client.Send(message);
            client.Disconnect(true);
        }

        public async Task<bool> SendingEmail(string recipients_address, Dictionary<string, string> replace_terms, string topic, EmailType type)
        {
            var htmlTemplate = LoadEmailTemplate(GetTemplateFileName(type));

            foreach (var term in replace_terms)
            {
                htmlTemplate = htmlTemplate.Replace($"{{{{{term.Key}}}}}", term.Value);
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["EMAIL:SENDER_NAME"], _configuration["EMAIL:SENDER_EMAIL"]));
            message.To.Add(new MailboxAddress(string.Empty, recipients_address));
            message.Subject = topic;
            var bodyBuilder = new BodyBuilder { HtmlBody = htmlTemplate };
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                await Task.Run(() => SendEmailViaSmtp(message));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        #endregion
    }
}
