using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Должность")]
        public string Position { get; set; }

        [Display(Name = "Ученая степень")]
        public string? Degree { get; set; }

        [Display(Name = "Кафедра")]
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
    }
}