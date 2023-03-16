namespace FGLogDog.RabbitMQ.Producer.Config
{
    internal interface IProducerConfiguration
    {
        string IpAddress { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Queue { get; set; }
    }
}
