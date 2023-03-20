namespace FGLogDog.UDP.Receiver.Config
{
    internal class ReceiverConfiguration : IReceiverConfiguration
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int SizeOfBuffer { get; set; }
    }
}
