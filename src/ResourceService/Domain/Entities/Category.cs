using SharedLibrary.SharedKernel.Entities;

namespace ResourceService.Domain.Entities
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
