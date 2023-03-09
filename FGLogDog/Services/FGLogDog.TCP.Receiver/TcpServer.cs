using FGLogDog.Application.Contracts;
using FGLogDog.FGLogDog.Application.Helper;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FGLogDog.TCP.Receiver
{
    internal class TcpServer : ITcpServer
    {
        private readonly ILogger _logger;

        public TcpServer(ILogger<TcpServer> logger)
            => _logger = logger;

        public async Task Start(IPAddress ipAddress, int port, ParserDelegate parse)
        {
            IPEndPoint ipPoint = new IPEndPoint(ipAddress, port);
            TcpListener tcpListener = new TcpListener(ipPoint);

            try
            {
                tcpListener.Start();
                _logger.LogInformation($"{DateTime.Now} LogDog started TCP server on {ipAddress}:{port}");

                while (true)
                {
                    using var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    var stream = tcpClient.GetStream();
                    byte[] responseData = new byte[1024];
                    int bytes = 0;
                    var response = new StringBuilder();
                    do
                    {
                        bytes = await stream.ReadAsync(responseData);
                        response.Append(Encoding.UTF8.GetString(responseData, 0, bytes));
                        await parse(response.ToString());
                    }
                    while(bytes > 0);

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