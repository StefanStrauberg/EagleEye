using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHostedService<Server>();
            return services;
        }
    }
}