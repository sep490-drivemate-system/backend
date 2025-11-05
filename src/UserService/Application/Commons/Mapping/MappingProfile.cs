using AutoMapper;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Application.Commons.DTOs.Instructors;
using UserService.Application.Commons.DTOs.NoviceDriver;
using UserService.Application.Commons.DTOs.Policy;
using UserService.Domain.Entities;

namespace UserService.Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, SignUpDTO>()
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.Role))
            .ReverseMap();
            CreateMap<User, UserClaimTokenDTO>().ReverseMap();

            CreateMap<(string AccessToken, string RefreshToken), SignInRespondDTO>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.AccessToken));

            CreateMap<Instructor, InstructorDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.ExperienceYear, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.BookingCount, opt => opt.Ignore())
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
            .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());

            CreateMap<string, SignUpRespondDTO>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src));

        }
    }
}
