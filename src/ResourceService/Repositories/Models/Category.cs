using Microsoft.Extensions.Hosting;
using SharedLibrary.SharedKernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Repositories.Models
{
    public class Category: BaseEntites
    {
        public string Name { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Quan hệ
        public ICollection<Blog> Blogs { get; set; }
    }
}
