using System.Net;

namespace Common
{
    public class Response<T>
    {
        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
