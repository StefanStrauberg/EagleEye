namespace WebAPI.EagleEye.Application.Exceptions
{
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message) 
            : base(message)
        {
        }
    }
}