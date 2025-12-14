using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Название кафедры обязательно")]
        [Display(Name = "Название кафедры")]
        public string Name { get; set; }

        [Display(Name = "Профиль")]
        public string? Profile { get; set; }

        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    }
}