using AutoMapper;
using ResourceService.Repositories.Models;
using ResourceService.Services.DTOs;

namespace ResourceService.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Blog -> ResourceDto
            CreateMap<Blog, ResourceDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            // Blog -> BlogDetailDto
            CreateMap<Blog, BlogDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents));

            // BlogContent -> BlogContentDto
            CreateMap<BlogContent, BlogContentDto>()
                .ForMember(dest => dest.Images, opt => opt.Ignore()); // Images đã bị xóa khỏi entity

            // Create mappings (DTO -> Entity)
            CreateMap<BlogContentCreateDto, BlogContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BlogId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

            CreateMap<BlogCreateDto, Blog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Update mappings (partial updates; ignore nulls)
            CreateMap<BlogContentUpdateDto, BlogContent>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); 

            CreateMap<BlogUpdateDto, Blog>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}


