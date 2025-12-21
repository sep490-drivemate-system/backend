using SharedLibrary.SharedKernel.Http.DTOs.Feedback;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Domain.Entities;

namespace UserService.Application.Commons.Mapping.ExtentionMapping
{
    public static class MappingFeedback
    {
        public static List<InstructorDTO> MapInstructorsWithFeedback(
            IEnumerable<Instructor> instructors, 
            FeedbackResponse feedbacks)
        {
            return instructors.Where(x => !x.IsDeleted).Select(instructor => 
            {
                var instructorFeedback = feedbacks?.InstructorStatistics?
                    .FirstOrDefault(f => f.InstructorId == instructor.Id);

                return new InstructorDTO
                {
                    Id = instructor.Id,
                    Avatar = instructor.User?.Avatar ?? "",
                    FullName = instructor.User?.Fullname ?? "",
                    ExperienceYear = instructor.Experience,
                    BookingCount = instructorFeedback?.BookingCount ?? 0,
                    AverageRating = instructorFeedback?.AverageRating ?? 0,
                };
            }).ToList();
        }
    }
}
