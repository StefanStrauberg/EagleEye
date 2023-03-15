using FGLogDog.Application.Contracts.Logger;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Logging
{
    public static class LoggerService
    {
        public static IServiceCollection AddLoggerServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IAppLogger<>), typeof(AppLogger<>));
            return services;
        }
    }
}