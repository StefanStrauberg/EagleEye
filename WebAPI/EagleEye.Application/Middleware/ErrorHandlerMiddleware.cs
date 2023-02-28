using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebAPI.EagleEye.Application.Exceptions;
using System.Text.Json;

namespace WebAPI.EagleEye.Application.Middleware
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
            => _next = next;
        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case BadRequestException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { statusCode = response.StatusCode, message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}