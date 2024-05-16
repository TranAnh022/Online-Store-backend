namespace Ecommerce.Service.src.Shared
{
    public class CustomExeption : Exception
    {
        public int StatusCode { get; set; }

        public CustomExeption(int statusCode, string msg) : base(msg)
        {
            StatusCode = statusCode;
        }

        public static CustomExeption NotFoundException(string msg = "Not found")
        {
            return new CustomExeption(404, msg);
        }

        public static CustomExeption BadRequestException(string msg = "Bad request")
        {
            return new CustomExeption(400, msg);
        }

        public static CustomExeption UnauthorizedException(string msg = "Unauthorized")
        {
            return new CustomExeption(401, msg);
        }

        public static CustomExeption ForbiddenException(string msg = "Forbidden")
        {
            return new CustomExeption(403, msg);
        }
    }
}