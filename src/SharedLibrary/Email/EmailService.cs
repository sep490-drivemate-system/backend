using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Resend;
using SharedLibrary.SharedKernel.Enum;
using System.Collections.Concurrent;

namespace SharedLibrary.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private static readonly ConcurrentDictionary<string, string> _templateCache = new();
        private readonly string _templateBasePath;
        private readonly bool _useResendApi;
        private readonly IResend? _resendClient;

        public EmailService(IConfiguration configuration, IResend? resendClient = null)
        {
            _templateBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "Template");
            _configuration = configuration;
            
            // Only use Resend if it's injected via DI
            // ResendClient requires IOptionsSnapshot and HttpClient, so it should be registered in DI
            _useResendApi = resendClient != null;
            
            if (_useResendApi)
            {
                _resendClient = resendClient;
            }
            
            Console.WriteLine($"[EmailService] Initialized. Using {(_useResendApi ? "Resend API" : "SMTP")} for email delivery.");
        }
        public async Task<bool> SendVerificationCodeAsync(string toEmail, string verificationCode)
        {
            try
            {
                Console.WriteLine($"[EmailService] Sending verification code to {toEmail}");
                var emailBody = GenerateEmailBody(toEmail, EmailType.VerifyOPTCode, null, verificationCode);
                var subject = GetEmailSubject(EmailType.VerifyOPTCode);
                
                if (_useResendApi)
                {
                    return await SendEmailViaResendApi(toEmail, subject, emailBody);
                }
                else
                {
                    var emailMessage = BuildEmailMessage(toEmail, emailBody, EmailType.VerifyOPTCode);
                    await SendEmailViaSmtp(emailMessage);
                    Console.WriteLine($"[EmailService] Verification code email sent successfully to {toEmail}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR sending verification code to {toEmail}: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[EmailService] Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[EmailService] Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
        public async Task SendForgotPasswordAsync(string toEmail, string resetToken)
        {
            try
            {
                Console.WriteLine($"[EmailService] Sending forgot password email to {toEmail}");
                var emailBody = GenerateEmailBody(toEmail, EmailType.ForgotPassword, resetToken, null);
                var subject = GetEmailSubject(EmailType.ForgotPassword);
                
                if (_useResendApi)
                {
                    await SendEmailViaResendApi(toEmail, subject, emailBody);
                }
                else
                {
                    var emailMessage = BuildEmailMessage(toEmail, emailBody, EmailType.ForgotPassword);
                    await SendEmailViaSmtp(emailMessage);
                    Console.WriteLine($"[EmailService] Forgot password email sent successfully to {toEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR sending forgot password email to {toEmail}: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[EmailService] Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        public async Task<bool> SendInstructorWelcomingAsync(string toEmail, DateOnly verificationExpirationDate)
        {
            try
            {
                Console.WriteLine($"[EmailService] Sending instructor welcome email to {toEmail}");
                var htmlTemplate = LoadEmailTemplate(GetTemplateFileName(EmailType.InstructorRegistration));

                string mesageBody = htmlTemplate.Replace("{{username}}", toEmail)
                    .Replace("{{expiry_date}}", verificationExpirationDate.ToString("dd/MM/yyyy"))
                    .Replace("{{login_url}}", _configuration["FRONTEND:LOGIN"]);

                var subject = GetEmailSubject(EmailType.InstructorRegistration);
                
                if (_useResendApi)
                {
                    return await SendEmailViaResendApi(toEmail, subject, mesageBody);
                }
                else
                {
                    var message = new MimeMessage();
                    var senderName = _configuration["EMAIL:SENDER_NAME"] ?? _configuration["EMAIL__SENDER_NAME"] ?? "DriveMate";
                    var senderEmail = _configuration["EMAIL:SENDER_EMAIL"] ?? _configuration["EMAIL__SENDER_EMAIL"];
                    message.From.Add(new MailboxAddress(senderName, senderEmail));
                    message.To.Add(new MailboxAddress(string.Empty, toEmail));
                    message.Subject = subject;
                    var bodyBuilder = new BodyBuilder { HtmlBody = mesageBody};
                    message.Body = bodyBuilder.ToMessageBody();
                    
                    await SendEmailViaSmtp(message);
                    Console.WriteLine($"[EmailService] Instructor welcome email sent successfully to {toEmail}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR sending instructor welcome email to {toEmail}: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[EmailService] Stack trace: {ex.StackTrace}");
                return false;
            }
        }


        #region "Behind the scene"

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
                EmailType.RejectSession => "RejectSession.html",
                EmailType.PostRejected => "PostRejected.html",
                EmailType.WithdrawRejected => "WithdrawRejected.html",
                EmailType.BanUser => "BanUser.html",
                EmailType.UnbanUser => "UnbanUser.html",
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
                EmailType.InstructorReschedule => "Thông báo yêu cầu đổi lịch hẹn",
                EmailType.DriverReschedule => "Thông báo thay đổi lịch hẹn",
                EmailType.RejectSession => "Thông báo: Học viên từ chối buổi thuê",
                EmailType.PostRejected => "Thông báo: Bài viết của bạn đã bị từ chối",
                EmailType.WithdrawRejected => "Thông báo: Yêu cầu rút tiền đã bị từ chối",
                EmailType.BanUser => "Tài khoản của bạn đã bị khóa",
                EmailType.UnbanUser => "Tài khoản của bạn đã được mở khóa",
                _ => "Thông báo từ DriveMate"
            };
        }

        private async Task SendEmailViaSmtp(MimeMessage message)
        {
            // Support both EMAIL:SMTP_* and EMAIL__SMTP_* formats (Railway uses __ which becomes : in code)
            var smtpServer = _configuration["EMAIL:SMTP_SERVER"] ?? _configuration["EMAIL__SMTP_SERVER"];
            var smtpPortStr = _configuration["EMAIL:SMTP_PORT"] ?? _configuration["EMAIL__SMTP_PORT"] ?? "587";
            var smtpPort = int.Parse(smtpPortStr);
            var senderEmail = _configuration["EMAIL:SENDER_EMAIL"] ?? _configuration["EMAIL__SENDER_EMAIL"];
            var senderPassword = _configuration["EMAIL:SENDER_PASSWORD"] ?? _configuration["EMAIL__SENDER_PASSWORD"];
            
            Console.WriteLine($"[EmailService] Attempting to connect to SMTP: {smtpServer}:{smtpPort}");
            Console.WriteLine($"[EmailService] From: {senderEmail}");
            Console.WriteLine($"[EmailService] To: {string.Join(", ", message.To.Select(t => t.ToString()))}");
            
            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
            {
                throw new InvalidOperationException($"SMTP configuration missing. Server: {smtpServer}, Email: {senderEmail}, Password: {(string.IsNullOrEmpty(senderPassword) ? "MISSING" : "SET")}");
            }
            
            using var client = new SmtpClient();
            client.Timeout = 30000; // 30 seconds timeout for SendGrid
            
            // For SendGrid, use port 587 with StartTls (most reliable)
            // If that fails, try port 465 with SSL
            var portsToTry = smtpServer.Contains("sendgrid", StringComparison.OrdinalIgnoreCase) 
                ? new[] { 587, 465 } // SendGrid: try 587 first, then 465
                : new[] { smtpPort, 465, 25 }; // Other SMTP: try configured port, then alternatives
            
            var sslOptions = smtpServer.Contains("sendgrid", StringComparison.OrdinalIgnoreCase)
                ? new[] { SecureSocketOptions.StartTls, SecureSocketOptions.SslOnConnect } // SendGrid: StartTls preferred
                : new[] { SecureSocketOptions.StartTls, SecureSocketOptions.SslOnConnect, SecureSocketOptions.Auto };
            
            Exception lastException = null;
            
            foreach (var port in portsToTry)
            {
                foreach (var sslOption in sslOptions)
                {
                    try
                    {
                        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                        Console.WriteLine($"[EmailService] Trying to connect to {smtpServer}:{port} with {sslOption}...");
                        
                        await client.ConnectAsync(smtpServer, port, sslOption, cts.Token);
                        Console.WriteLine($"[EmailService] Connected successfully to {smtpServer}:{port}");
                        
                        Console.WriteLine($"[EmailService] Authenticating with username: {senderEmail}...");
                        await client.AuthenticateAsync(senderEmail, senderPassword, cts.Token);
                        Console.WriteLine($"[EmailService] Authenticated successfully");
                        
                        Console.WriteLine($"[EmailService] Sending email...");
                        await client.SendAsync(message, cts.Token);
                        Console.WriteLine($"[EmailService] Email sent successfully");
                        
                        await client.DisconnectAsync(true, cts.Token);
                        Console.WriteLine($"[EmailService] Disconnected from SMTP server");
                        return; // Success, exit method
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"[EmailService] Connection to {smtpServer}:{port} timed out");
                        lastException = new TimeoutException($"SMTP connection to {smtpServer}:{port} timed out after 30 seconds.");
                        if (client.IsConnected)
                        {
                            await client.DisconnectAsync(false);
                        }
                        continue; // Try next port/option
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[EmailService] Failed to connect to {smtpServer}:{port} with {sslOption}: {ex.GetType().Name} - {ex.Message}");
                        lastException = ex;
                        if (client.IsConnected)
                        {
                            try { await client.DisconnectAsync(false); } catch { }
                        }
                        continue; // Try next port/option
                    }
                }
            }
            
            // If we get here, all attempts failed
            Console.WriteLine($"[EmailService] All SMTP connection attempts failed");
            throw lastException ?? new InvalidOperationException($"Failed to connect to SMTP server {smtpServer} on any port");
        }

        private async Task<bool> SendEmailViaResendApi(string toEmail, string subject, string htmlBody)
        {
            if (_resendClient == null)
            {
                throw new InvalidOperationException("Resend client is not configured. Please set RESEND_API_KEY or RESEND_APITOKEN environment variable.");
            }

            try
            {
                var fromEmail = _configuration["EMAIL:SENDER_EMAIL"] ?? _configuration["EMAIL__SENDER_EMAIL"] ?? "noreply@yourdomain.com";
                var fromName = _configuration["EMAIL:SENDER_NAME"] ?? _configuration["EMAIL__SENDER_NAME"] ?? "DriveMate";

                Console.WriteLine($"[EmailService] Sending email via Resend API to {toEmail}");

                var message = new EmailMessage
                {
                    From = $"{fromName} <{fromEmail}>",
                    To = new[] { toEmail },
                    Subject = subject,
                    HtmlBody = htmlBody
                };

                var response = await _resendClient.EmailSendAsync(message);

                if (response != null )
                {
                    Console.WriteLine($"[EmailService] Email sent successfully via Resend API to {toEmail}");
                    return true;
                }
                else
                {
                    var errorMsg = "Unknown error";
                    Console.WriteLine($"[EmailService] Resend API error: {errorMsg}");
                    throw new HttpRequestException($"Resend API returned error: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR sending email via Resend API to {toEmail}: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[EmailService] Stack trace: {ex.StackTrace}");
                throw;
            }
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
                Console.WriteLine($"[EmailService] Sending email to {recipients_address}");
                
                if (_useResendApi)
                {
                    return await SendEmailViaResendApi(recipients_address, topic, htmlTemplate);
                }
                else
                {
                    await SendEmailViaSmtp(message);
                    Console.WriteLine($"[EmailService] Email sent successfully to {recipients_address}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] ERROR sending email to {recipients_address}: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine($"[EmailService] Stack trace: {ex.StackTrace}");
                return false;
            }
        }
        #endregion
    }
}
