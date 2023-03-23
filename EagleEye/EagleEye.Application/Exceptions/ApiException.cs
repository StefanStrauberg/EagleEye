using System;
using System.Globalization;

namespace WebAPI.EagleEye.Application.Exceptions
{
    internal class ApiException : Exception
    {
        public ApiException() : base() {}

        public ApiException(string message) : base(message) { }

        public ApiException(string message, params object[] args) 
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}