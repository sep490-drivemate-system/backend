namespace UserService.Application.Commons.DTOs.Auth
{
    public class SignInRespondDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
