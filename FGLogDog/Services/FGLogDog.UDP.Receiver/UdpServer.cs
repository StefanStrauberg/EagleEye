using FGLogDog.Application.Contracts;
using FGLogDog.FGLogDog.Application.Helper;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FGLogDog.UDP.Receiver
{
    internal class UdpServer : IUdpServer
    {   
        private readonly ILogger _logger;

        public UdpServer(ILogger<UdpServer> logger)
            => _logger = logger;

        public async Task Start(IPAddress ipAddress, int port, ParserDelegate parse)
        {
            IPEndPoint ipPoint = new IPEndPoint(ipAddress, port);
            UdpClient udpClient = new UdpClient(ipPoint);
            IPEndPoint RemoteIpEndPoint = null;
            
            _logger.LogInformation($"{DateTime.Now} LogDog started UDP server on {ipAddress}:{port}");

            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string message = Encoding.UTF8.GetString(receiveBytes);
                    await parse(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} LogDog server stoped.\n{ex.Message}");
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}