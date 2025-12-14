using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.Models
{
    public class Publication
    {
        [Key]
        public int PublicationId { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Год")]
        public int Year { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}