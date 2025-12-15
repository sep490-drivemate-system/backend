using AutoMapper;
using SharedLibrary.SharedKernel.Http.DTOs.Instructor;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Domain.Entities;

namespace UserService.Application.Commons.Mapping.ExtentionMapping
{
    public static class InstructorMappingExtensions
    {
        public static List<InstructorDTO> MapWithStatistics(
            this IEnumerable<Instructor> instructors,
            IMapper mapper,
            Dictionary<Guid, InstructorOverviewFeedbackResponse> feedbackStats)
        {
            return instructors.Select(instructor =>
            {
                var instructorDTO = mapper.Map<InstructorDTO>(instructor);
                if (feedbackStats.TryGetValue(instructor.Id, out var stats))
                {
                    instructorDTO.AverageRating = stats.AverageRating;
                    instructorDTO.BookingCount = stats.BookingCount;
                    instructorDTO.PackageCount = stats.PackageCount;
                }
                else
                {
                    instructorDTO.AverageRating = 0;
                    instructorDTO.BookingCount = 0;
                    instructorDTO.PackageCount = 0;
                }

                return instructorDTO;
            }).ToList();
        }
    }
}
