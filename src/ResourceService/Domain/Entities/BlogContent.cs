using SharedLibrary.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Domain.Entities
{
    public class BlogContent: BaseEntites
    {
        public Guid BlogId { get; set; }
        public string Content { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Navigation property
        public Blog Blog { get; set; }
    }
}
