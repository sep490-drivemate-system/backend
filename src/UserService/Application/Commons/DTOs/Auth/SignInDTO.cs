namespace UserService.Application.Commons.DTOs.Auth
{
    public class SignInDTO
    {
        public string EmailOrPhone { get; set; }
        public string Password { get; set; }
    }

    public class TestDto
    {
        public testccdmat testccdmat { get; set; }
        public testccdmas testccdmas { get; set; }

    }

    public class testccdmat
    {
        public int cccdmt { get; set; }
        public IFormFile formFilecccd { get; set; }
    }
    public class testccdmas
    {

        public int cccdms { get; set; }
        public IFormFile formFilecccdms { get; set; }
    }
}
