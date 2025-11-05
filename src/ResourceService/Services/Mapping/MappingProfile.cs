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

            // BlogImage -> ResourceImageDto
            CreateMap<BlogImage, ResourceImageDto>()
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.ImageUrl));

            // Blog -> BlogDetailDto
            CreateMap<Blog, BlogDetailDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents));

            // BlogContent -> BlogContentDto
            CreateMap<BlogContent, BlogContentDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            // Create mappings (DTO -> Entity)
            CreateMap<BlogImageCreateDto, BlogImage>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ContentId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

            CreateMap<BlogContentCreateDto, BlogContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BlogId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));

            CreateMap<BlogCreateDto, Blog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Update mappings (partial updates; ignore nulls)
            CreateMap<BlogImageUpdateDto, BlogImage>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<BlogContentUpdateDto, BlogContent>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<BlogUpdateDto, Blog>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}


