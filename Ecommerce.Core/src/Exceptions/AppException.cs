using System.Net;

namespace Ecommerce.Core.src.Common
{
    public class AppException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        public static AppException BadRequest(string message = "Bad Request")
        {
            return new AppException
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message
            };
        }
    }
}