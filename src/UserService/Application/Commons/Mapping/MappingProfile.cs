using AutoMapper;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Application.Commons.DTOs.Users;
using UserService.Domain.Entities;

namespace UserService.Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, SignUpDTO>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role));
            //.ReverseMap();

            CreateMap<SignUpDTO, User>()
                .ForMember(dest => dest.Role, act => act.MapFrom(src => src.UserRole))
                .ForMember(dest => dest.Fullname, act => act.MapFrom(src => src.FullName));

            CreateMap<string, SignUpRespondDTO>()
                .ForMember(dest => dest.Token, act => act.MapFrom(src => src));

            CreateMap<User, UserClaimTokenDTO>().ReverseMap();

            CreateMap<(string AccessToken, string RefreshToken), SignInRespondDTO>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.AccessToken));

            CreateMap<(string AccessToken, string RefreshToken), SignUpRespondDTO>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.AccessToken));

            CreateMap<Instructor, InstructorDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Fullname))
            .ForMember(dest => dest.ExperienceYear, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.User.Gender))
            .ForMember(dest => dest.BookingCount, opt => opt.Ignore())
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore());

            CreateMap<Instructor, InstructorDetailDTO>()
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.ExperienceYear, opt => opt.MapFrom(src => src.Experience));

            CreateMap<NoviceDriver, NoviceDriverDetailDTO>()
                .ForMember(dest => dest.NoviceDriverId, opt => opt.MapFrom(src => src.Id));

            CreateMap<User, UserDetailDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Fullname))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.LicenseTier, opt => opt.MapFrom(src => src.MaxLicenseLevel))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.Instructor, opt =>
                {
                    opt.PreCondition(src => src.Role == UserRole.Instructor && src.Instructor != null);
                    opt.MapFrom(src => src.Instructor);
                })
                .ForMember(dest => dest.NoviceDriver, opt =>
                {
                    opt.PreCondition(src => src.Role == UserRole.NoviceDriver && src.NoviceDriver != null);
                    opt.MapFrom(src => src.NoviceDriver);
                });

            CreateMap<SavedLocation, UserAddressDTO>()
                .ForMember(dest => dest.AddressString, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.LocationLatitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.LocationLongtitude));

            CreateMap<PersonalSchedule, InstructorScheduleDTO>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
        }
    }
}
