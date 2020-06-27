using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CPK.Sso.Models.ManageViewModels
{
    public class UserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Имя")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Display(Name = "Email подтвержден")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Роли")]
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        
        [Display(Name = "Доступные роли")]
        public IEnumerable<string> AvailableRoles { get; set; } = new List<string>();
    }
}
