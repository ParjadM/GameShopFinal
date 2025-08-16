namespace GameShop.Models
{
    // Represents the error view model used in the application
    public class ErrorViewModel
    {
        // unique request identifier
        public string? RequestId { get; set; }
        // True if the request ID is not null or empty
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
