using System.Net;

namespace AzureBlobProject.Models.DTO
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Result { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
