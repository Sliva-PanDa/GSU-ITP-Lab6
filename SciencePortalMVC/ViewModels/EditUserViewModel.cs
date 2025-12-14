using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SciencePortalMVC.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress]
        public string Email { get; set; }

        public string UserName { get; set; }

    }
}