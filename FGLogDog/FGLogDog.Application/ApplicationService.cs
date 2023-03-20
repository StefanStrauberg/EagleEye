using FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CommonFilter>(
                configuration.GetSection("ServiceConfiguration").GetSection("CommonFilter"));
            services.AddSingleton<ICommonFilter>(provider =>
                provider.GetRequiredService<IOptions<CommonFilter>>().Value);

            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddHostedService<Server>();
            return services;
        }
    }
}