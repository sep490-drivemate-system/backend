namespace SharedLibrary.SharedKernel.Http.DTOs.User
{
    public class InstructorBasicInfoDTO
    {
        public Guid InstructorId { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
