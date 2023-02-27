using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FGLogDog.Application.Commands;
using FGLogDog.Application.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FGLogDog.FGLogDog.Application.Services
{
    public class TcpServer : ITcpServer
    {
        private readonly string _input;
        private readonly int _buferSize;
        private readonly ILogger _logger;
        private readonly IPEndPoint _localIpEndPoint;
        private readonly IMediator _mediator;
        public TcpServer(IConfiguration configuration,
                         ILogger<TcpServer> logger,
                         IMediator mediator)
        {
            _logger = logger;
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _localIpEndPoint = new IPEndPoint(ParserFactory.SearchSubStringIP(_input, "srcip=", ParserTypes.IP),
                                              ParserFactory.SearchSubstringINT(_input, "srcport=", ParserTypes.INT));
            _buferSize = ParserFactory.SearchSubstringINT(_input, "bufersize=", ParserTypes.INT);
            _mediator = mediator;
        }

        public async Task Start()
        {
            using var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(_localIpEndPoint);

            try
            {
                while (true)
                {
                    EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = new byte[_buferSize];
                    var returnData = await tcpSocket.ReceiveFromAsync(receiveBytes, RemoteIpEndPoint);
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