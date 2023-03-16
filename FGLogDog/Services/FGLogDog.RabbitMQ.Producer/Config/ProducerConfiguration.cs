namespace FGLogDog.RabbitMQ.Producer.Config
{
    internal class ProducerConfiguration : IProducerConfiguration
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }
    }
}
