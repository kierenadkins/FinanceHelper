namespace Web.Models.Auth
{
    public class SignupViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public ErrorViewModel Errors { get; set; } = new ErrorViewModel();
    }
}
