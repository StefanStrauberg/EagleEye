using FGLogDog.Application.Contracts;
using FGLogDog.Application.Features.Commands;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : IServer
    {
        private readonly IMediator _mediator;
        private readonly string _input;
        private readonly IUdpServer _udpServer;
        private readonly ITcpServer _tcpServer;

        public Server(IConfiguration configuration,
                      IMediator mediator,
                      IUdpServer udpServer,
                      ITcpServer tcpServer)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _mediator = mediator;
            _udpServer = udpServer;
            _tcpServer = tcpServer;
        }

        public async Task StartServer()
        {
            var typeOfServer = Enum.Parse<TypeOfReciver>(ParserFactory.GetSTRING(_input, "protocol="));

            switch (typeOfServer)
            {
                case TypeOfReciver.udp:
                    await ServerRun(_udpServer);
                    break;
                case TypeOfReciver.tcp:
                    await ServerRun(_tcpServer);
                    break;
                default:
                throw new ArgumentException($"Invalid incomming type of protocol: {typeOfServer}.");
            }
        }

        private async Task ServerRun<T>(T server) where T : IReciver<TcpUdpReciverParams>
            => await server.Run(new TcpUdpReciverParams(_input, Parser));

        private async Task Parser(string message)
            => await _mediator.Send(new ParseLogCommand(message.ToString()));

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}