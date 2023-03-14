using FGLogDog.Application.Contracts.Producer;
using RabbitMQ.Client;
using System.Text;

namespace FGLogDog.RabbitMQ.Producer
{
    internal class MessageProducer : IRabbitMQProducer
    {
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory 
            { 
                HostName = "localhost" 
            };

            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("fortigate", exclusive: false);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "fortigate", body: body);
        }
    }
}