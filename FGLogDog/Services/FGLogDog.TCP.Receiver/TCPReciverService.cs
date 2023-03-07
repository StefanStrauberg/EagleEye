using FGLogDog.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.TCP.Receiver
{
    public static class TCPReciverService
    {
        public static IServiceCollection AddTCPReciverServices(this IServiceCollection services)
        {
            services.AddScoped<ITcpServer, TcpServer>();
            return services;
        }
    }
}