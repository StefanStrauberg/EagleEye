namespace FGLogDog.TCP.Receiver.Config
{
    internal interface IReceiverConfiguration
    {
        string IpAddress { get; set; }
        int Port { get; set; }
    }
}