namespace Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public List<string> Errors { get; set; } = new List<string>();
        public ErrorViewModel() { }

        public ErrorViewModel(IEnumerable<string>? errors)
        {
            if (errors != null)
                Errors.AddRange(errors);
        }
        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                Errors.Add(error);
        }
        public bool HasErrors => Errors.Any();
    }
}
