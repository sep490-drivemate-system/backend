namespace SharedLibrary.SharedKernel.Http.DTOs.User
{
    public class NoviceDriverBasicInfoDTO
    {
        public Guid NoviceDriverId { get; set; }
        public string Fullname { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
    }
}
