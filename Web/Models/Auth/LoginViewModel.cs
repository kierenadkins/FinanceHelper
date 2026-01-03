namespace Web.Models.Auth
{
    public class LoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string? Error { get; set; }
    }
}
