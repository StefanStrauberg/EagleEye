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
                HostName = parameters.IpAddress.ToString(),
                Port = parameters.Port,
                UserName = parameters.Username,
                Password = parameters.Password
            };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: parameters.Queue,
                                         durable: false,
                                         autoDelete: false,
                                         exclusive: false,
                                         arguments: null);

            _logger.LogInformation($"LogDog started rabbitmq producer to {parameters.IpAddress}:{parameters.Port}");

            try
            {
                while (true)
                {
                    var message = parameters.getMessage()
                                            .ToJson();
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                         routingKey: parameters.Queue,
                                         body: body);
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