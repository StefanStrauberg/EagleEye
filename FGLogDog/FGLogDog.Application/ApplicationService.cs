using FGLogDog.Application.Contracts;
using FGLogDog.FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });
            services.AddSingleton<IServer, Server>();
            return services;
        }
    }
}