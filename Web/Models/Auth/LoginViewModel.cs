using System.ComponentModel.DataAnnotations;

namespace Web.Models.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        public string? Error { get; set; }
    }
}
