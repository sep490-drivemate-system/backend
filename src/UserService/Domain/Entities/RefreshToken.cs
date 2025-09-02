using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class RefreshToken : BaseEntites
    {
        public string RefreshKey { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
