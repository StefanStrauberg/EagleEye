using EagleEye.Application.Contracts.Logger;
using EagleEye.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.EagleEye.Logging
{
    public static class LoggingService
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
    }
}
