using ResourceService.Domain.Enums;
using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
{
    public class Blog: BaseEntites
    {
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ImageList { get; set; } // JSON array of image URLs
        public Guid? CategoryId { get; set; }
        public BlogStatus Status { get; set; } 
        public DateTime? UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Quan hệ
        public Category? Category { get; set; }
        public ICollection<BlogContent> Contents { get; set; }
        
    }
}
