using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.TCP.Receiver.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FGLogDog.TCP.Receiver
{
    public static class TCPReciverService
    {
        public static IServiceCollection AddTCPReciverServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ReceiverConfiguration>(
                configuration.GetSection("ServiceConfiguration").GetSection("Receiver").GetSection("tcp"));
            services.AddSingleton<IReceiverConfiguration>(provider =>
                provider.GetRequiredService<IOptions<ReceiverConfiguration>>().Value);
                
            services.AddSingleton<ITCPReceiver, TCPServer>();

            return services;
        }
    }
}