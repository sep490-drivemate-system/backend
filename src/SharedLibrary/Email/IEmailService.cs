using SharedLibrary.SharedKernel.Enum;

namespace SharedLibrary.Email
{
    public interface IEmailService
    {
        Task<bool> SendingEmail(string recipients_address, Dictionary<string, string> replace_terms, string topic, EmailType type);
        bool SendVerificationCodeAsync(string toEmail, string verificationCode);
        Task SendForgotPasswordAsync(string toEmail, string resetToken);
        Task<bool> SendInstructorWelcomingAsync(string toEmail, DateOnly verificationExpirationDate);
    }
}
