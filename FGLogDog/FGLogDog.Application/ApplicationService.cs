using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        /// <summary>
        /// Adding main logic to start producer and receiver
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHostedService<Server>();
            return services;
        }
    }
}