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

            CreateMap<User, SignUpDTO>().ReverseMap();
            CreateMap<User, UserClaimTokenDTO>().ReverseMap();

            CreateMap<Policy, PolicyDTO>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Detail, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.PolicyType));

            CreateMap<(string AccessToken, string RefreshToken), SignInRespondDTO>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));

            CreateMap<Address, UserAddressDTO>()
            .ForMember(dest => dest.AddressString, opt => opt.MapFrom(src => src.Location));

            CreateMap<Instructor, InstructorDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.ExperienceYear, opt => opt.MapFrom(src => src.Experience))
            .ForMember(dest => dest.BookingCount, opt => opt.Ignore())
            .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
            .ForMember(dest => dest.UnitPrice, opt => opt.Ignore());
        }
    }
}
