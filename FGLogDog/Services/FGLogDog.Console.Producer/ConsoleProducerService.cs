using FGLogDog.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Console.Producer
{
    public static class ConsoleProducerService
    {
        public static IServiceCollection AddUDPReciverServices(this IServiceCollection services)
        {
            services.AddScoped<IConsoleProducer, ConsoleProducer>();
            return services;
        }
    }
}
