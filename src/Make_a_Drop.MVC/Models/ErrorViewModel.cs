namespace Make_a_Drop.MVC.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public string? StackTrace { get; internal set; }
    public int? StatusCode { get; internal set; }
    public string? Message { get; internal set; }
}

