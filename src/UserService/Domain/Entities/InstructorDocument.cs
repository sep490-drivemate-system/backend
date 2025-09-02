using SharedLibrary.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class InstructorDocument : BaseEntites
    {
        public Guid ApplicationId { get; set; }
        public Guid TypeId { get; set; }
        public string Note {  get; set; }
        public DocumentStatus status { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDeleTe { get; set; }
        [ForeignKey(nameof(TypeId))]
        public virtual DocumentType DocumentType { get; set; }
    }
}
