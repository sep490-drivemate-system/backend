using SharedLibrary.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Repositories.Models
{
    public class BlogImage:BaseEntites
    {

        public Guid ContentId { get; set; }
        public string ImageUrl { get; set; }
        public int No { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Quan hệ
        public BlogContent BlogContent { get; set; }
    }
}
