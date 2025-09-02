using SharedLibrary.SharedKernel.Entities;

namespace UserService.Domain.Entities
{
    public class Address : BaseEntites
    {
        public Guid UserId { get; set; }
        public string Location {  get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleTe { get; set; }
    }
}
