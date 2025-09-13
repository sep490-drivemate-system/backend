using SharedLibrary.SharedKernel.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Enum;

namespace UserService.Domain.Entities
{
    public class InstructorDocument : BaseEntites
    {
        public string ImageURL {  get; set; }
        public string Note {  get; set; }
        public DocumentStatus Status { get; set; }
        public DateTime UpdateAt { get; set; }
        public bool IsDelete { get; set; }

        // Key for relationship
        public Guid ApplicationId { get; set; }
        public Guid TypeId { get; set; }


        // Relationship
        public virtual DocumentType? DocumentType { get; set; }
        public virtual InstructorApplication? InstructorApplication { get; set; }
    }
}
