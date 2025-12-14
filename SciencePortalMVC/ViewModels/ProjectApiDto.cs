using System;
using System.ComponentModel.DataAnnotations;

namespace SciencePortalMVC.ViewModels
{
    public class ProjectApiDto
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string FundingOrg { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LeaderName { get; set; } 
    }

    // DTO для создания нового проекта (POST)
    public class CreateProjectApiDto
    {
        [Required(ErrorMessage = "Название проекта обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Номер проекта обязателен")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Финансирующая организация обязательна")]
        public string FundingOrg { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Руководитель проекта обязателен")]
        public int LeaderId { get; set; }
    }

    // DTO для обновления существующего проекта (PUT)
    public class UpdateProjectApiDto
    {
        public int ProjectId { get; set; } 

        [Required(ErrorMessage = "Название проекта обязательно")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Номер проекта обязателен")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Финансирующая организация обязательна")]
        public string FundingOrg { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Руководитель проекта обязателен")]
        public int LeaderId { get; set; }
    }
}