using FGLogDog.Application.Contracts.Server;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.ComponentsOfServer
{
    public static class ComponentsOfServerService
    {
        /// <summary>
        /// Adding switch logic to starting type of producer and type of receiver to the application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddComponentsOfServerServices(this IServiceCollection services)
        {
            services.AddSingleton<ITypeOfComponentsServer, TypeOfComponentsServer>();
            return services;
        }
    }
}