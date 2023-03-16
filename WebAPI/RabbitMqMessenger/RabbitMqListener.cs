using EagleEye.Application.Contracts.Logger;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.EagleEye.RabbitMqMessenger.RabbitMQConfig;

namespace WebAPI.EagleEye.RabbitMqMessenger
{
    internal class RabbitMqListener : BackgroundService
    {
        readonly IMessengerConnection _messengerConnection;
        readonly IConnection _connection;
        readonly IModel _channel;
        readonly IAppLogger<RabbitMqListener> _logger;

        public RabbitMqListener(IAppLogger<RabbitMqListener> logger, IMessengerConnection messengerConnection)
        {
            _messengerConnection = messengerConnection;
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
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                // TODO push input log to local API buffer
                _logger.LogInformation(content);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_messengerConnection.Queue, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
