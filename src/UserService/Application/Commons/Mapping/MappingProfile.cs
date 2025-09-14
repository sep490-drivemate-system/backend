using AutoMapper;
using UserService.Application.Commons.DTOs.Auth;
using UserService.Domain.Entities;

namespace UserService.Application.Commons.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<User, SignUpDTO>().ReverseMap();

        }
    }
}
