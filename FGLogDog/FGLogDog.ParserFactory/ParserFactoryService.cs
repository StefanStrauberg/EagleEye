using FGLogDog.Application.Contracts.Filter;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.ParserFactory
{
    public static class ParserFactoryService
    {
        public static IServiceCollection AddParserFactoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IParserFactory, GeneralParserFactory>();
            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddSingleton<ICommonFilter, CommonFilter>();
            return services;
        }
    }
}