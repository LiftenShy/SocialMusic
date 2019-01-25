using System.ComponentModel.DataAnnotations;

namespace AccountManager.Data.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
