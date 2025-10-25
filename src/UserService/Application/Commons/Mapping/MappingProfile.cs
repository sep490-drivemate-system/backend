using AutoMapper;
using UserService.Application.Commons.DTOs.Auth;
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

        }
    }
}
