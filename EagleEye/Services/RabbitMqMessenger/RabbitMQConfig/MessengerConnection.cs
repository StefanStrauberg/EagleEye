namespace WebAPI.EagleEye.RabbitMqMessenger.RabbitMQConfig
{
    internal class MessengerConnection : IMessengerConnection
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Queue { get; set; }
    }
}