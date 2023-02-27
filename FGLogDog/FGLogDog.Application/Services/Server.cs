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
        private readonly IMediator _mediator;
        private readonly string _input;
        private readonly string _typeOfServer;
        private readonly IPAddress _localIPAddress;
        private readonly int _localPort;
        private readonly int _buferSize;

        public Server(IConfiguration configuration,
                      ILogger<Server> logger,
                      IMediator mediator)
        {
            _configuration = configuration;
            _logger = logger;
            _mediator = mediator;
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _typeOfServer = ParserFactory.SearchSubStringSTRING(_input, "protocol=");
            _localIPAddress = ParserFactory.SearchSubStringIP(_input, "srcip=");
            _localPort = ParserFactory.SearchSubstringINT(_input, "srcport=");
            _buferSize = ParserFactory.SearchSubstringINT(_input, "bufersize=");
        }

        public async Task StartServer()
        {
            switch (_typeOfServer)
            {
                case "udp;":
                    using (IUdpServer udpServer = new UdpServer(new LoggerFactory().CreateLogger<UdpServer>(), _mediator))
                    {
                        CreateLog(_typeOfServer);
                        await udpServer.Start(_localIPAddress, _localPort, _buferSize);
                    }
                    break;
                case "tcp;":
                    using (ITcpServer udpServer = new TcpServer(new LoggerFactory().CreateLogger<TcpServer>(),_mediator))
                    {
                        CreateLog(_typeOfServer);
                        await udpServer.Start(_localIPAddress, _localPort, _buferSize);
                    }
                    break;
                default:
                throw new ArgumentException("Invalid incomming type of protocol: {0}.", _typeOfServer);
            }
        }

        private void CreateLog(string serverType)
        {
            _logger.LogInformation("{0} LogDog {1} Server started on {2}:{3}", DateTime.Now, serverType, _localIPAddress, _localPort);
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}