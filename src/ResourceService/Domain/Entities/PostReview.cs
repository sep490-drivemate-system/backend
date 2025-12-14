using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class PostReview : BaseEntites
    {
        public Guid PostId { get; set; }
        public string? Reason { get; set; }          
        public Guid? ReviewerId { get; set; }       
        public DateTime ReviewedAt { get; set; }    
        public bool IsDeleted { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public virtual Post? Post { get; set; }
    }
}

