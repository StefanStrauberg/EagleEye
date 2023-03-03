using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using WebAPI.EagleEye.Application.Exceptions;

namespace WebAPI.EagleEye.Application.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = GetStatusCode(ex);

            await response.WriteAsync(new ApiResponse(statusCode: response.StatusCode,
                                                      message: ex.Message).ToString());
        }

        private int GetStatusCode(Exception ex)
            => ex switch
            {
                BadRequestException => (int)HttpStatusCode.BadRequest,
                NotFoundException => (int)HttpStatusCode.NotFound,
                _ => (int)HttpStatusCode.InternalServerError
            };
    }
}