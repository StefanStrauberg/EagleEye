using FGLogDog.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.Console.Producer
{
    public static class ConsoleProducerService
    {
        public static IServiceCollection AddConsoleProducerServices(this IServiceCollection services)
        {
            services.AddSingleton<IConsoleProducer, ConsoleProducer>();
            return services;
        }
    }
}
