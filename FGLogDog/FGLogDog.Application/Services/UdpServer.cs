using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FGLogDog.Application.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class UdpServer : IUdpServer
    {   
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public UdpServer(ILogger<UdpServer> logger,
                         IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Start(IPAddress ipAddress, int port, int buferSize)
        {
            using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            var localIpEndPoint = new IPEndPoint(ipAddress, port);
            
            udpSocket.Bind(localIpEndPoint);

            try
            {
                while (true)
                {
                    EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = new byte[buferSize];
                    var returnData = await udpSocket.ReceiveFromAsync(receiveBytes, RemoteIpEndPoint);
                    var message = Encoding.UTF8.GetString(receiveBytes, 0, returnData.ReceivedBytes);
                    // Send message to MediatR
                    await _mediator.Send(new ParseLogCommand(message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} LogDog server stoped.\n{1}", DateTime.Now, ex.Message);
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}