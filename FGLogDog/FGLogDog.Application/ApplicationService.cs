using System.Reflection;
using FGLogDog.Application.DataStore;
using FGLogDog.Application.Services;
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
            services.AddSingleton<IUdpReceiver,UdpReceiver>();
            return services;
        }
    }
}