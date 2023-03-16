using FGLogDog.FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddHostedService<Server>();
            return services;
        }
    }
}