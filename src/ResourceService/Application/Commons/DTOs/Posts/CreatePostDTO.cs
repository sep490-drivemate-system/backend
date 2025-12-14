using Microsoft.AspNetCore.Http;

namespace ResourceService.Application.Commons.DTOs.Posts
{
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<IFormFile>? Images { get; set; }
        public List<int>? ImageOrders { get; set; }
        public List<IFormFile>? Videos { get; set; }
        public List<int>? VideoOrders { get; set; }

        public List<Guid>? TagIds { get; set; }
        public List<Guid>? CategoryIds { get; set; }
    }

  
}

