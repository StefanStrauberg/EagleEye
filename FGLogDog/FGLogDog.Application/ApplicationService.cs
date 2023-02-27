using System.Reflection;
using FGLogDog.FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddLogging(configure => configure.AddConsole());
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddSingleton<IUdpServer, UdpServer>();
            services.AddSingleton<ITcpServer, TcpServer>();
            services.AddSingleton<IServer, Server>();
            return services;
        }
    }
}