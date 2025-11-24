using AutoMapper;
using BookingService.Application.Commons.DTOs.Booking;
using BookingService.Application.Commons.DTOs.Cars.Get;
using BookingService.Application.Commons.DTOs.DrivingSkills;
using BookingService.Application.Commons.DTOs.DrivingSessions;
using BookingService.Application.Commons.DTOs.Feedbacks;
using BookingService.Application.Commons.DTOs.Package;
using BookingService.Application.Commons.DTOs.RoadTypes;
using BookingService.Domain.Entities;
using SharedLibrary.SharedKernel.Http.DTOs.Package;
using BookingService.Application.Commons.DTOs.DrivingSession;

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


            CreateMap<Domain.Entities.DrivingSkill, DrivingSkillDTO>();
            CreateMap<Domain.Entities.RoadType, RoadTypeDTO>();
            CreateMap<Booking, PackageBuyingDTO>().ReverseMap();

            // Mapping for Package DTOs
            CreateMap<Package, PackageDto>()
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration.ToString()))
                .ForMember(dest => dest.IsRentalCar, opt => opt.MapFrom(src => src.Cars != null && src.Cars.Any()))
                .ForMember(dest => dest.RoadTypes, opt => opt.MapFrom(src => 
                    src.RoadTypes != null ? src.RoadTypes.Select(r => r.Name).ToList() : new List<string>()))
                .ForMember(dest => dest.DrivingSkills, opt => opt.MapFrom(src => 
                    src.DrivingSkills != null ? src.DrivingSkills.Select(d => d.Name).ToList() : new List<string>()));


            // Mapping for Car DTOs
            CreateMap<Car, CarInstructorDetailDTO>()
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SeatCounts, opt => opt.MapFrom(src => src.SeatCount))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));

            // Mapping for Instructor Schedule DTOs
            CreateMap<DrivingSession, InstructorScheduleDTO>();

            CreateMap<DrivingSession, DrivingSessionlDTO>().ReverseMap();

                // Mapping for DrivingSessionDTO
            CreateMap<DrivingSession, DrivingSessionDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.PackageName, opt => opt.MapFrom(src => src.Booking != null && src.Booking.Package != null ? src.Booking.Package.Name : ""))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.StatusDisplayString, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PickupLocation, opt => opt.MapFrom(src => src.DisplayStartLocationName ?? ""))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.StartingLatitude))
                .ForMember(dest => dest.Longtitude, opt => opt.MapFrom(src => src.StartingLongtitude));

            CreateMap<Booking, BookingsDTO>()
                           .ForMember(dest => dest.NamePackage,
                               opt => opt.MapFrom(src => src.Package != null ? src.Package.Name : ""))
                           .ForMember(dest => dest.BookingStatus,
                               opt => opt.MapFrom(src => src.Status))
                           .ForMember(dest => dest.BuyDate,
                               opt => opt.MapFrom(src => src.CreatedAt))
                            .ForMember(dest => dest.Price,
                               opt => opt.MapFrom(src => src.PriceAtBuyingTime))
                           .ForMember(dest => dest.Duration,
                               opt => opt.MapFrom(src => (int)src.DurationWhenBought))
                           .ForMember(dest => dest.DurationInUse,
                               opt => opt.MapFrom(src => src.CalculateDurationUsed()))
                           .ForMember(dest => dest.RemainingTime,
                               opt => opt.MapFrom(src => src.CalculateRemainingTime()))
                           .ForMember(dest => dest.PrecentInUse,
                               opt => opt.MapFrom(src => src.CalculatePercentInUse()))
                           .ForMember(dest => dest.InstructorId,
                               opt => opt.MapFrom(src => src.InstructorId))
                           .ForMember(dest => dest.RoadTypes,
                               opt => opt.MapFrom(src => src.Package != null && src.Package.RoadTypes != null 
                                   ? src.Package.RoadTypes.Select(rt => rt.Name).ToList()
                                   : new List<string>()))
                           .ForMember(dest => dest.DrivingSkills,
                               opt => opt.MapFrom(src => src.Package != null && src.Package.DrivingSkills != null 
                                   ? src.Package.DrivingSkills.Select(ds => ds.Name).ToList()
                                   : new List<string>()));

            // Mapping for Feedback
            CreateMap<FeedbackCreationDTO, Feedback>()
                .ForMember(dest => dest.CarRating, opt => opt.MapFrom(src => src.CarRating ?? 0))
                .ForMember(dest => dest.CarFeedback, opt => opt.MapFrom(src => src.CarFeedback ?? ""))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId ?? Guid.Empty))
                .ForMember(dest => dest.Booking, opt => opt.Ignore())
                .ForMember(dest => dest.Car, opt => opt.Ignore());

            // Mapping for DrivingSessionScheduleDTO
            CreateMap<DrivingSession, DrivingSessionScheduleDTO>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
        }
    }
}
