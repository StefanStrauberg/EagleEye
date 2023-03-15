using FGLogDog.Application.Contracts.Reciver;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.UDP.Receiver
{
    public static class UDPReciverService
    {
        public static IServiceCollection AddUDPReciverServices(this IServiceCollection services)
        {
            services.AddSingleton<IUdPReceiver, UdpServer>();
            return services;
        }
    }
}