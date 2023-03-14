using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Features.Commands;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : IServer
    {
        readonly IMediator _mediator;
        readonly string _input;
        readonly string _output;
        readonly IUdpServer _udpServer;
        readonly IConsoleProducer _consoleProducer;

        public Server(IConfiguration configuration,
                      IMediator mediator,
                      IUdpServer udpServer,
                      IConsoleProducer consoleProducer)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _output = configuration.GetSection("ConfigurationString").GetSection("Output").Value;
            _mediator = mediator;
            _udpServer = udpServer;
            _consoleProducer = consoleProducer;
        }

        public Task StartServer()
        {
            var typeOfReciver = Enum.Parse<TypeOfReciver>(ParserFactory.GetSTRING(_input, "protocol="));
            var typeOfProducer = Enum.Parse<TypeOfProducer>(ParserFactory.GetSTRING(_output, "protocol="));

            switch (typeOfReciver)
            {
                case TypeOfReciver.udp:
                    new Task(async () => await ReciverRun(_udpServer)).Start();
                    break;
                default:
                throw new ArgumentException($"Invalid incomming type of input protocol: {typeOfReciver}.");
            }

            switch (typeOfProducer)
            {
                case TypeOfProducer.console:
                    new Task(async () => await ProducerRun(_consoleProducer)).Start();
                    break;
                default:
                throw new ArgumentException($"Invalid incomming type of output protocol: {typeOfProducer}.");
            }

            return Task.CompletedTask;
        }

        private async Task ReciverRun<T>(T reciver) where T : IReciver<TcpUdpReciverParams>
            => await reciver.Run(new TcpUdpReciverParams(_input, Parser));

        private async Task ProducerRun<T>(T producer) where T : IProducer<ConsoleProducerParams>
            => await producer.Run(new ConsoleProducerParams(Producer));

        private async Task Parser(string message)
            => await _mediator.Send(new ParseLogCommand(message.ToString()));

        private async Task<BsonDocument> Producer()
            => await _mediator.Send(new GetMessageCommand());

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}