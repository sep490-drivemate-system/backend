using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ResourceService.Application.Commons.DTOs
{
    // Read DTOs
    public class BlogDetailDto
    {
        public Guid Id { get; set; }
        public Guid InstructorId { get; set; }
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public IList<string> ImageList { get; set; } // List of image URLs
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Status { get; set; } // BlogStatus enum value
        // Changed from BlogContentDto to string so API returns plain content instead of nested object
        public string Content { get; set; } // Single content text (1 blog = 1 content)
    }

    public class BlogContentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    }

    // Create DTOs
    public class BlogCreateDto
    {
        public string Title { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid CategoryId { get; set; }
        public IList<BlogContentCreateDto> Contents { get; set; }
        public IList<string> ImageUrls { get; set; } // URLs after upload
    }

    public class BlogContentCreateDto
    {
        public string Content { get; set; }
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
        public bool? IsDeleted { get; set; }
    }

    public class BlogStatusDto
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

    // Request model for CreateBlog (multipart/form-data)
    public class BlogCreateRequest
    {
        public string Title { get; set; }
        public IFormFile Thumbnail { get; set; } // Thumbnail image file
        public Guid CategoryId { get; set; }
        public string Content { get; set; } // Single content text (1 blog = 1 content)
        public IFormFileCollection Images { get; set; } // List of additional images
    }

    // Request model for UpdateBlog (multipart/form-data)
    public class BlogUpdateRequest
    {
        public string Title { get; set; }
        public IFormFile Thumbnail { get; set; } // Thumbnail image file (optional - only if updating)
        public Guid? CategoryId { get; set; }
        public string Contents { get; set; } // JSON string of BlogContentUpdateDto array
        public IFormFileCollection Images { get; set; } // New images to add (optional)
    }
}


