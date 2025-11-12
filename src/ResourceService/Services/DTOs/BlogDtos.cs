using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ResourceService.Services.DTOs
{
    // Read DTOs
    public class BlogDetailDto
    {
        public Guid Id { get; set; }
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<BlogContentDto> Contents { get; set; }
    }

    public class BlogContentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int No { get; set; }
        public IList<ResourceImageDto> Images { get; set; }
    }

    // Create DTOs
    public class BlogCreateDto
    {
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid CategoryId { get; set; }
        public IList<BlogContentCreateDto> Contents { get; set; }
    }

    public class BlogContentCreateDto
    {
        public string Content { get; set; }
        public int No { get; set; }
        public string ImageUrl { get; set; }
    }


    // Update DTOs (nullable cho partial updates)
    public class BlogUpdateDto
    {
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid? CategoryId { get; set; }
        public IList<BlogContentUpdateDto> Contents { get; set; }
    }

    public class BlogContentUpdateDto
    {
        public Guid? Id { get; set; }
        public string Content { get; set; }
        public int? No { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsDeleted { get; set; }
    }
}


