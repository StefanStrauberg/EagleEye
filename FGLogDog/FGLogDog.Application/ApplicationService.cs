using FGLogDog.Application.Contracts.Buffer;
using FGLogDog.Application.Contracts.Filter;
using FGLogDog.Application.Contracts.Parser;
using FGLogDog.Application.Contracts.Server;
using FGLogDog.FGLogDog.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITypeOfServer, TypeOfServer>();
            services.AddSingleton<ICommonFilter, CommonFilter>();
            services.AddSingleton<IParserFactory, ParserFactory>();
            services.AddSingleton<IBufferRepository, BufferRepository>();
            services.AddSingleton<IConfigurationFilters, ConfigurationFilters>();
            services.AddHostedService<Server>();
            return services;
        }
    }
}