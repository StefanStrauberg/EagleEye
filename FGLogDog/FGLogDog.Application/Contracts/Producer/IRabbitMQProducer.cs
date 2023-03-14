namespace FGLogDog.Application.Contracts.Producer
{
    public interface IRabbitMQProducer
    {
        void SendMessage(string message);
    }
}
