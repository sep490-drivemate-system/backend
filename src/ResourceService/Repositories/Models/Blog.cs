using ResourceService.Repositories.Enum;
using SharedLibrary.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Repositories.Models
{
    public class Blog: BaseEntites
    {
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid CategoryId { get; set; }
        public BlogStatus Status { get; set; } 
        public DateTime? UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Quan hệ
        public Category Category { get; set; }
        public ICollection<BlogContent> Contents { get; set; }
        
    }
}
