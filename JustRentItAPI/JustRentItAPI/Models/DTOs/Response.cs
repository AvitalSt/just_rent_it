using System.Net;

namespace JustRentItAPI.Models.DTOs
{
    public class Response
    {
        public bool IsSuccess { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; } = string.Empty;
    }

    public class Response<T> : Response
    {
        public T? Data { get; set; }
    }
}
