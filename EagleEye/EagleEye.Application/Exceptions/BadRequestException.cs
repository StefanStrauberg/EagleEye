namespace WebAPI.EagleEye.Application.Exceptions
{
    internal class BadRequestException : ApiException
    {
        public BadRequestException(string message) 
            : base(message)
        {
        }
    }
}