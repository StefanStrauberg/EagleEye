using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.UDP.Receiver.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FGLogDog.UDP.Receiver
{
    public static class UDPReceiverService
    {
        /// <summary>
        /// Adding UDP receiver to the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddUDPReciverServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ReceiverConfiguration>(
                configuration.GetSection("ServiceConfiguration").GetSection("Receiver").GetSection("udp"));
            services.AddSingleton<IReceiverConfiguration>(provider =>
                provider.GetRequiredService<IOptions<ReceiverConfiguration>>().Value);

            services.AddSingleton<IUDPReceiver, UDPServer>();

            return services;
        }
    }
}