using System.Reflection;
using FGLogDog.Application.Contracts;
using FGLogDog.FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddLogging(configure => configure.AddConsole());
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<IServer, Server>();
            return services;
        }
    }
}