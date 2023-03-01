using EagleEye.Application.Contracts.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.EagleEye.Logging
{
    public static class LoggingService
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            return services;
        }
    }
}
