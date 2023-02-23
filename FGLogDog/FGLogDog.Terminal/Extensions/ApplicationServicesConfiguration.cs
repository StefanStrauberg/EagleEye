using FGLogDog.Application;
using FGLogDog.Application.Handlers;
using FGLogDog.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Terminal.Extensions
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging(configure => configure.AddConsole())
                    .AddTransient<UdpServer>()
                    .AddTransient<ParseFGLogHandler>();
            services.AddApplicationServices();
        }
    }
}