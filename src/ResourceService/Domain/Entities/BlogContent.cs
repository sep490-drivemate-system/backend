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
        public int No { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }


        public Blog Blog { get; set; }
        
    }
}
