using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.ViewModels
{
    // --- DTO для Кафедр ---
    public class DepartmentApiDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string? Profile { get; set; }
    }

    public class CreateUpdateDepartmentDto
    {
        [Required]
        public string Name { get; set; }
        public string? Profile { get; set; }
    }

    // --- DTO для Публикаций ---
    public class PublicationApiDto
    {
        public int PublicationId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public List<string> AuthorNames { get; set; } = new();
    }

    public class CreateUpdatePublicationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Year { get; set; }
    }
}