using System;
using System.Text.Json;

namespace EagleEye.Application.Exceptions
{
    public class ApiResponse
    {
        private int _statusCode;
        private string _message;

        public ApiResponse(int statusCode, string message = null)
        {
            _statusCode = statusCode;
            _message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get => _statusCode; }
        public string Message { get => _message; }

        private string GetDefaultMessageForStatusCode(int statusCode)
            => statusCode switch
            {
                400 => "You have made bad request.",
                401 => "You're not Authorized.",
                404 => "Resource was not found.",
                500 => "Something go wrong please tell about it your software developer.",
                _ => null
            };

        public override string ToString()
            => JsonSerializer.Serialize(this);
    }
}