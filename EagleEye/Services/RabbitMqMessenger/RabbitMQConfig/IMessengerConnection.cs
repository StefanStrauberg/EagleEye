namespace WebAPI.EagleEye.RabbitMqMessenger.RabbitMQConfig
{
    internal interface IMessengerConnection
    {
        string IpAddress { get; set; }
        int Port { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Queue { get; set; }
    }
}