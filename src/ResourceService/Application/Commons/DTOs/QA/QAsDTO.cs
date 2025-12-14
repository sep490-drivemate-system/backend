using ResourceService.Domain.Enums;

namespace ResourceService.Application.Commons.DTOs.QA
{
    public class QAsDTO
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyCollection<QaItemDto> Items { get; set; } = Array.Empty<QaItemDto>();
    }

    public class QaItemDto
    {
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public QaStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}

