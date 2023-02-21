using System.Reflection;
using FGLogDog.Application.DataStore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddSingleton<FakeDataStore>();
            return services;
        }
    }
}