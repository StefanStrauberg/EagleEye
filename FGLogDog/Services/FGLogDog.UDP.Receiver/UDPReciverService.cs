using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.UDP.Receiver.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FGLogDog.UDP.Receiver
{
    public static class UDPReciverService
    {
        public static IServiceCollection AddUDPReciverServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ReceiverConfiguration>(
                configuration.GetSection("ServiceConfiguration").GetSection("Receiver").GetSection("udp"));
            services.AddSingleton<IReceiverConfiguration>(provider =>
                provider.GetRequiredService<IOptions<ReceiverConfiguration>>().Value);

            services.AddSingleton<IUdPReceiver, UdpServer>();

            return services;
        }
    }
}