using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Application.Commons.DTOs
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public string CategoryName { get; set; }
        public int Status { get; set; } // BlogStatus enum value
    }
}
