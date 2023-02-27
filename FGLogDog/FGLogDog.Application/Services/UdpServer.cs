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

        public async Task Start(IPAddress ipAddress, int port)
        {
            UdpClient udpClient = new UdpClient(port);
            IPEndPoint RemoteIpEndPoint = null;

            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string message = Encoding.UTF8.GetString(receiveBytes);
                    await _mediator.Send(new ParseLogCommand(message.ToString()));
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