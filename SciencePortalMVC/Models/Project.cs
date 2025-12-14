using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Номер")]
        public string Number { get; set; }

        [Required]
        [Display(Name = "Организация-спонсор")]
        public string FundingOrg { get; set; }

        [Required]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Руководитель")]
        public int LeaderId { get; set; }
        public virtual Teacher? Leader { get; set; }
    }
}