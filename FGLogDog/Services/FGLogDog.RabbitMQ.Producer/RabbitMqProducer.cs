using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Models;
using FGLogDog.RabbitMQ.Producer.Config;
using MongoDB.Bson;
using RabbitMQ.Client;
using System;
using System.Text;

namespace FGLogDog.RabbitMQ.Producer
{
    internal class RabbitMqProducer : IRabbitMQProducer
    {
        readonly IProducerConfiguration _producerConfiguration;
        readonly IAppLogger<RabbitMqProducer> _logger;
        IConnection _connection;
        IModel _channel;
        bool _isBrocken;
        
        public RabbitMqProducer(IAppLogger<RabbitMqProducer> logger, IProducerConfiguration producerConfiguration)
        {
            _logger = logger;
            _producerConfiguration = producerConfiguration;
        }

        void IProducer.Run(ProducerParameters parameters)
        {
            Initialize();
            try
            {
                if (!_isBrocken)
                {
                    while (true)
                    {
                        var message = parameters.getMessage().ToJson();
                        var body = Encoding.UTF8.GetBytes(message);
                        _channel.BasicPublish(exchange: "",
                                              routingKey: _producerConfiguration.Queue,
                                              basicProperties: null,
                                              body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        void Initialize()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _producerConfiguration.IpAddress,
                    Port = _producerConfiguration.Port,
                    UserName = _producerConfiguration.UserName,
                    Password = _producerConfiguration.Password
                };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: _producerConfiguration.Queue,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                _logger.LogInformation($"LogDog started rabbitmq producer to {_producerConfiguration.IpAddress}:{_producerConfiguration.Port}");
            }
            catch (Exception ex)
            {
                _logger.LogError(GetExceptionMessages(ex, nameof(RabbitMqProducer)));
                _isBrocken = true;
            }
        }

        static string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }

        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}