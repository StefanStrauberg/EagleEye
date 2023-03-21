using FGLogDog.Application.Contracts.Server;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.ComponentsOfServer
{
    public static class ComponentsOfServerService
    {
        public static IServiceCollection AddComponentsOfServerServices(this IServiceCollection services)
        {
            services.AddSingleton<ITypeOfComponentsServer, TypeOfComponentsServer>();
            return services;
        }
    }
}