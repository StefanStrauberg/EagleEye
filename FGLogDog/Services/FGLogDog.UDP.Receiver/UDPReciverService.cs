using FGLogDog.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.UDP.Receiver
{
    public static class UDPReciverService
    {
        public static IServiceCollection AddUDPReciverServices(this IServiceCollection services)
        {
            services.AddSingleton<IUdpServer, UdpServer>();
            return services;
        }
    }
}