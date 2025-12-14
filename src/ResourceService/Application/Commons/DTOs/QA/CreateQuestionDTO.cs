namespace ResourceService.Application.Commons.DTOs.QA
{
    public class CreateQuestionDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Guid>? TagIds { get; set; }
    }
}

