namespace FGLogDog.UDP.Receiver.Config
{
    /// <summary>
    /// Configuration for receiver
    /// </summary>
    internal interface IReceiverConfiguration
    {
        /// <summary>
        /// The IP Address that will be used by the socket for to listen an incoming connection
        /// </summary>
        /// <value></value>
        string IpAddress { get; set; }
        /// <summary>
        /// The port number that will be used by the socket for to listen for an incoming connection
        /// </summary>
        /// <value></value>
        int Port { get; set; }
        /// <summary>
        /// The number of bytes in the incoming message
        /// </summary>
        /// <value></value>
        int SizeOfBuffer { get; set; }
    }
}
