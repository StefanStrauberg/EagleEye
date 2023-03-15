using FGLogDog.Application.Contracts.Producer;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.RabbitMQ.Producer
{
    public static class RabbitMQProducerService
    {
        public static IServiceCollection AddRabbitMQProducerServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQProducer, RabbitMqProducer>();
            return services;
        }
    }
}
