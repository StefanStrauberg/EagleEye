using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Commands;
using FGLogDog.Application.Services.Managers;
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
            services.AddSingleton<IBufferManager, BufferManager>();
            services.AddSingleton<IParserManager, ParserManager>();
            services.AddSingleton<IServer, Server>();
            return services;
        }
    }
}