using System.ComponentModel.DataAnnotations;

namespace CPK.Sso.Models.ManageViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool EmailSent { get; set; }
    }
}
