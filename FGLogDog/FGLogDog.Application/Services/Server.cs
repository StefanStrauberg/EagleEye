using System;
using System.Net;
using System.Threading.Tasks;
using FGLogDog.Application.Contracts;
using FGLogDog.Application.Features.Commands;
using FGLogDog.Application.Helper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : IServer
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly string _input;
        private readonly string _typeOfServer;
        private readonly IPAddress _localIPAddress;
        private readonly int _localPort;
        private readonly IUdpServer _udpServer;
        private readonly ITcpServer _tcpServer;

        public Server(IConfiguration configuration,
                      IMediator mediator,
                      IUdpServer udpServer,
                      ITcpServer tcpServer)
        {
            _configuration = configuration;
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _typeOfServer = ParserFactory.SearchSubStringSTRING(_input, "protocol=");
            _localIPAddress = ParserFactory.SearchSubStringIP(_input, "srcip=");
            _localPort = ParserFactory.SearchSubstringINT(_input, "srcport=");
            _mediator = mediator;
            _udpServer = udpServer;
            _tcpServer = tcpServer;
        }

        public async Task StartServer()
        {
            switch (_typeOfServer)
            {
                case "udp":
                    await _udpServer.Start(_localIPAddress, _localPort, Parser);
                    break;
                case "tcp":
                    await _tcpServer.Start(_localIPAddress, _localPort, Parser);
                    break;
                default:
                throw new ArgumentException("Invalid incomming type of protocol: {0}.", _typeOfServer);
            }
        }

        private async Task Parser(string message)
            => await _mediator.Send(new ParseLogCommand(message.ToString()));

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}