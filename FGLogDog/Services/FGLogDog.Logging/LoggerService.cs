using FGLogDog.Application.Contracts.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Logging
{
    public static class LoggerService
    {
        public static IServiceCollection AddLoggerServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
    }
}