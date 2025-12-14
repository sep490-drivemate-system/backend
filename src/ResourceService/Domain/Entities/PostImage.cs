namespace ResourceService.Domain.Entities
{
    public class PostImage : SharedLibrary.SharedKernel.Entities.BaseEntites
    {
        public Guid PostId { get; set; }
        public string Url { get; set; }
        public int? Order { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation
        public virtual Post? Post { get; set; }
    }
}
