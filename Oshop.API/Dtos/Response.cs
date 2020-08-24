using System.Net;

namespace Oshop.API.Dtos
{
    public class Response<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }

    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}