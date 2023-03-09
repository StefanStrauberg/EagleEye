using System.Net;

namespace FGLogDog.Application.Models
{
    public class TcpUdpReciverParams : ReciverParams
    {
        public IPAddress ipAddress { get; set; }
        public int port { get; set; }
    }
}