﻿using FGLogDog.Application.Contracts.Producer;
using FGLogDog.RabbitMQ.Producer.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FGLogDog.RabbitMQ.Producer
{
    public static class RabbitMQProducerService
    {
        /// <summary>
        /// Adding RabbitMQ producer to the application
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddRabbitMQProducerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ProducerConfiguration>(
                configuration.GetSection("ServiceConfiguration").GetSection("Producer").GetSection("amqp"));
            services.AddSingleton<IProducerConfiguration>(provider =>
                provider.GetRequiredService<IOptions<ProducerConfiguration>>().Value);

            services.AddSingleton<IRabbitMQProducer, RabbitMqProducer>();

            return services;
        }
    }
}
