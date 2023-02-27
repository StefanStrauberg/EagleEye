using System;
using System.Net;
using System.Threading.Tasks;
using FGLogDog.Application.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FGLogDog.FGLogDog.Application.Services
{
    public class Server : IServer
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMediator _mediator;
        private readonly string _input;
        private readonly string _typeOfServer;
        private readonly IPAddress _localIPAddress;
        private readonly int _localPort;

        public Server(IConfiguration configuration,
                      IMediator mediator,
                      ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<Server>();
            _mediator = mediator;
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _typeOfServer = ParserFactory.SearchSubStringSTRING(_input, "protocol=");
            _localIPAddress = ParserFactory.SearchSubStringIP(_input, "srcip=");
            _localPort = ParserFactory.SearchSubstringINT(_input, "srcport=");
        }

        public async Task StartServer()
        {
            switch (_typeOfServer)
            {
                case "udp":
                    using (IUdpServer udpServer = new UdpServer(_loggerFactory.CreateLogger<UdpServer>(), _mediator))
                    {
                        CreateInformationLog(_typeOfServer);
                        await udpServer.Start(_localIPAddress, _localPort);
                    }
                    break;
                case "tcp":
                    using (ITcpServer udpServer = new TcpServer(_loggerFactory.CreateLogger<TcpServer>(),_mediator))
                    {
                        CreateInformationLog(_typeOfServer);
                        await udpServer.Start(_localIPAddress, _localPort);
                    }
                    break;
                default:
                throw new ArgumentException("Invalid incomming type of protocol: {0}.", _typeOfServer);
            }
        }

        private void CreateInformationLog(string serverType)
        {
            _logger.LogInformation("{0} LogDog started {1} server on {2}:{3}", DateTime.Now, serverType, _localIPAddress, _localPort);
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}