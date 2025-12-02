namespace UserService.Application.Commons.DTOs.Documents
{
    public class SystemDocumentDTO
    {
        public string Name { get; set; }
        public IEnumerable<string> Items { get; set; }
        public string Type { get; set; }
    }

    public class SystemDocumentViewDTO: SystemDocumentDTO
    {
        public Guid Id { get; set; }
    }
}
