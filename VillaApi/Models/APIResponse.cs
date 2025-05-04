using System.Net;

namespace VillaApi.Models
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
    }
}
