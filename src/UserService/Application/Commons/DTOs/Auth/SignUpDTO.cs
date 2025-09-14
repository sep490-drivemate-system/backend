namespace UserService.Application.Commons.DTOs.Auth
{
    public class EmailDTO
    {
        public string Email { get; set; }
    }
    public class SignUpDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
