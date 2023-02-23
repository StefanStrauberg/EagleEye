using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Queries;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FGLogDog.Application.Services
{
    public class UdpServer : IUdpServer
    {   
        private readonly string _input;
        private readonly string _srcip;
        private readonly int _srcport;
        private readonly ILogger _logger;
        private readonly int _buferSize;
        private readonly IPEndPoint _localIpEndPoint;
        private readonly IMediator _mediator;

        public UdpServer(IConfiguration configuration,
                           ILogger<UdpServer> logger,
                           IMediator mediator)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            ParserFactory.SearchSubstring(_input, "srcip=", ParserTypes.IP, out _srcip);
            ParserFactory.SearchSubstring(_input, "srcport=", ParserTypes.INT, out _srcport);
            ParserFactory.SearchSubstring(_input, "bufersize=", ParserTypes.INT, out _buferSize);
            _logger = logger;
            _localIpEndPoint = new IPEndPoint(IPAddress.Parse(_srcip), _srcport);
            _mediator = mediator;
        }

        public async Task ServerStart()
        {
            using var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(_localIpEndPoint);
            try
            {
                _logger.LogInformation("{0} LogDog server start on ip:{1} and port:{2}",
                                       DateTime.Now,
                                       _srcip,
                                       _srcport);
                while (true)
                {
                    EndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] receiveBytes = new byte[_buferSize];
                    var returnData = await udpSocket.ReceiveFromAsync(receiveBytes, RemoteIpEndPoint);
                    var message = Encoding.UTF8.GetString(receiveBytes, 0, returnData.ReceivedBytes);
                    // Send message to MediatR
                    await _mediator.Send(new ParseFGLogQuery(message));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} LogDog server stoped.\n{1}", DateTime.Now, ex.Message);
            }
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}