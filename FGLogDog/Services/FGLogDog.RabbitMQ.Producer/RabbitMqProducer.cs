using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.FGLogDog.Application.Models.ParametersOfProducers;
using MongoDB.Bson;
using RabbitMQ.Client;
using System;
using System.Text;

namespace FGLogDog.RabbitMQ.Producer
{
    internal class RabbitMqProducer : IRabbitMQProducer
    {
        readonly IAppLogger<RabbitMqProducer> _logger;
        
        public RabbitMqProducer(IAppLogger<RabbitMqProducer> logger)
            => _logger = logger;

        void IProducer<RabbitMQProducerParams>.Run(RabbitMQProducerParams parameters)
        {
            var factory = new ConnectionFactory
            {
                HostName = parameters.ipAddress.ToString(),
                Port = parameters.port
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            _logger.LogInformation($"LogDog started rabbitmq producer to {parameters.ipAddress}:{parameters.port}");

            try
            {
                while (true)
                {
                    var message = parameters.getMessage()
                                            .ToJson();
                    channel.QueueDeclare("fortigate", exclusive: false);
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "", routingKey: "fortigate", body: body);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"LogDog reciver stoped.\n{ex.Message}");
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}