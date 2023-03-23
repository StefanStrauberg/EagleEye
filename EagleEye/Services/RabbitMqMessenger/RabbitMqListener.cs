using EagleEye.Application.Contracts.Logger;
using EagleEye.Application.Contracts.TemporaryBuffer;
using EagleEye.Application.Exceptions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.RabbitMqMessenger.RabbitMQConfig;

namespace WebAPI.EagleEye.RabbitMqMessenger
{
    internal class RabbitMqListener : BackgroundService
    {
        readonly IMessengerConnection _messengerConnection;
        readonly IAppLogger<RabbitMqListener> _logger;
        readonly IBufferRepository _bufferRepository;
        IConnection _connection;
        IModel _channel;

        public RabbitMqListener(IAppLogger<RabbitMqListener> logger,
                                IMessengerConnection messengerConnection,
                                IBufferRepository bufferRepository)
        {
            _messengerConnection = messengerConnection;
            _logger = logger;
            _bufferRepository = bufferRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            try
            {
                Initialize();

                var consumer = new EventingBasicConsumer(_channel);

                consumer.Received += (model, eventArgs) =>
                {
                    byte[] body = eventArgs.Body.ToArray();

                    _bufferRepository.PushToBuffer(body);

                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                };

                _channel.BasicConsume(_messengerConnection.Queue, false, consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ConvertExceptionMessage.GetExceptionMessages(ex, nameof(RabbitMqListener)));
            }
            finally
            {
                (this as IDisposable).Dispose();
            }
            return Task.CompletedTask;
        }

        void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = _messengerConnection.IpAddress,
                Port = _messengerConnection.Port,
                UserName = _messengerConnection.UserName,
                Password = _messengerConnection.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _messengerConnection.Queue,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            _logger.LogInformation($"EagleEye started rabbitmq listener from {_messengerConnection.IpAddress}:{_messengerConnection.Port}");
        }

        public override void Dispose()
            => GC.SuppressFinalize(this);
    }
}
