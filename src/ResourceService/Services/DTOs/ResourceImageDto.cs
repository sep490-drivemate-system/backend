using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceService.Services.DTOs
{
    public class ResourceImageDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public int No { get; set; }
    }
}
