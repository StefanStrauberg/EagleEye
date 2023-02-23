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
    public class UdpServer : IUdpServer
    {   
        private readonly string _input;
        private readonly string _srcip;
        private readonly string _srcport;
        private readonly ILogger _logger;
        private const int bufSize = 8 * 1024;
        private readonly IPEndPoint _localIpEndPoint;

        public UdpServer(IConfiguration configuration,
                           ILogger<UdpServer> logger)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            ParserFactory.SearchSubstring(_input, "srcip=", ParserTypes.IP, out _srcip);
            ParserFactory.SearchSubstring(_input, "srcport=", ParserTypes.INT, out _srcport);
            _logger = logger;
            _localIpEndPoint = new IPEndPoint(IPAddress.Parse(_srcip), Int32.Parse(_srcport));
        }

        public async Task ServerStart()
        {
            using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(_localIpEndPoint);
            try
            {
                _logger.LogInformation($"{DateTime.Now} LogDog server start on ip:{_srcip} and port:{_srcport}");
                while (true)
                {
                    EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = new byte[bufSize];
                    var returnData = await udpSocket.ReceiveFromAsync(receiveBytes, RemoteIpEndPoint);
                    var message = Encoding.UTF8.GetString(receiveBytes, 0, returnData.ReceivedBytes);
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} Something go wrong the LogDog server crashed.\r\n{0}", ex.Message);
            }
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}