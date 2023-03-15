using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using Microsoft.Extensions.DependencyInjection;

namespace FGLogDog.RabbitMQ.Producer
{
    public static class RabbitMQProducerService
    {
        public static IServiceCollection AddRabbitMQRServices(this IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQProducer, RabbitMqProducer>();
            return services;
        }
    }
}
