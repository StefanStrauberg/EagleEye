using System;

namespace WebAPI.EagleEye.Application.Exceptions
{
    public class BadRequestException : AppException
    {
        public BadRequestException(string message) 
            : base(message)
        {
        }
    }
}