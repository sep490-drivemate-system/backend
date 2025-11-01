using AutoMapper;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Domain.Entities;

namespace BookingService.Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //// Mapping BookingDTO to Booking entity
            //CreateMap<BookingDTO, Booking>()
            //    .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
            //    .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
            //    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Domain.Enum.BookingStatus.Planned))
            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            //    .ForMember(dest => dest.TimeRanges, opt => opt.Ignore())
            //    .ForMember(dest => dest.DrivingSessions, opt => opt.Ignore())
            //    .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
            //    .ForMember(dest => dest.DrivingSkills, opt => opt.Ignore())
            //    .ForMember(dest => dest.RoadTypes, opt => opt.Ignore());

            //// Mapping TimeRangeRequest DTO to TimeRange entity
            //CreateMap<TimeRangeRequest, TimeRange>()
            //    .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => DateOnly.MinValue.ToDateTime(src.StartTime)))
            //    .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => DateOnly.MinValue.ToDateTime(src.EndTime)))
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            //    .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
            //    .ForMember(dest => dest.BookingId, opt => opt.Ignore())
            //    .ForMember(dest => dest.Bookings, opt => opt.Ignore());


            CreateMap<DrivingSkill, DrivingSkillDTO>();
            CreateMap<RoadType, RoadTypeDTO>();

        }
    }
}
