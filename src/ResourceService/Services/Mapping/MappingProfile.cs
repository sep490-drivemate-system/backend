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

            // Category -> CategoryDto
            CreateMap<Category, CategoryDto>();

            // Create mappings (DTO -> Entity)
            CreateMap<BlogContentCreateDto, BlogContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BlogId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore());

            // BlogContentUpdateDto -> BlogContent (for creating new content)
            CreateMap<BlogContentUpdateDto, BlogContent>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BlogId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content != null ? src.Content.Trim() : string.Empty))
                .ForMember(dest => dest.No, opt => opt.MapFrom(src => src.No ?? 0))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? string.Empty));

            CreateMap<BlogCreateDto, Blog>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UpdateAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsDelete, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Contents))
                .ForMember(dest => dest.Category, opt => opt.Ignore());

        }
    }
}


