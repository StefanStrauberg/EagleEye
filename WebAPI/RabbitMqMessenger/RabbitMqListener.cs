using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace RabbitMqMessenger
{
    internal class RabbitMqListener : BackgroundService
    {
        readonly IConnection _connection;
        readonly IModel _channel;

        public RabbitMqListener(IConnection connection, IModel channel)
        {
            var factory = new ConnectionFactory 
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin-password"
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "FG6H0ETB20901717",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                Debug.WriteLine(content);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("FG6H0ETB20901717", false, consumer);

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
