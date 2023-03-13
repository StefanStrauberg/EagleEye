using System.Net;
using FGLogDog.Application.Helper;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public class TcpUdpReciverParams : ReciverParameters
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;

        public TcpUdpReciverParams(string configuration, ParserDelegate parser)
            : base(parser)
        {
            _ipAddress = ParserFactory.GetIP(configuration, "srcip=");
            _port = ParserFactory.GetINT(configuration, "srcport=");
        }

        public IPAddress ipAddress { get => _ipAddress; }
        public int port { get => _port; }
    }
}