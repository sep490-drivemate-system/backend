using System;

namespace ResourceService.Application.Commons.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; }
    }
}

