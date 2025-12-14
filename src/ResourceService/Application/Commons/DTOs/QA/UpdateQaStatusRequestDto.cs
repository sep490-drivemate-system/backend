using ResourceService.Domain.Enums;

namespace ResourceService.Application.Commons.DTOs.QA
{
    public class UpdateQaStatusRequestDto
    {
        public QaStatus Status { get; set; }
        public string? Reason { get; set; }
        public Guid ReviewerId { get; set; }
    }
}

