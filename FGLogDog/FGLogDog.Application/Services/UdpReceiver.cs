using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Application.Services
{
    public class UdpReceiver : IUdpReceiver
    {
        private readonly string _input;
        private readonly string _srcip;
        private readonly string _srcport;
        private readonly ILogger<UdpReceiver> _logger;

        public UdpReceiver(IConfiguration configuration,
                           ILogger<UdpReceiver> logger)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            ParserFactory.SearchSubstring(_input, "srcip=", ParserTypes.IP, out _srcip);
            ParserFactory.SearchSubstring(_input, "srcport=", ParserTypes.INT, out _srcport);
            _logger = logger;
        }

        public async Task Start()
        {
            using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var localIP = new IPEndPoint(IPAddress.Parse(_srcip), Int32.Parse(_srcport));
            udpSocket.Bind(localIP);

            _logger.LogInformation($"LogDog server ready to recive messages on ip:{_srcip} and port:{_srcport}");

            byte[] data = new byte[256];
            EndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
            var result = await udpSocket.ReceiveFromAsync(data, remoteIp);
            var message = Encoding.UTF8.GetString(data, 0, result.ReceivedBytes);
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}