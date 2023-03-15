using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Commands;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Models.ParametersOfProducers;
using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : IServer
    {
        readonly string _input;
        readonly string _output;
        readonly IUdpServer _udpServer;
        readonly IConsoleProducer _consoleProducer;
        readonly IBufferManager _bufferManager;
        readonly IParserManager _parserManager;
        readonly IRabbitMQProducer _rabbitMQProducer;

        public Server(IConfiguration configuration,
                      IUdpServer udpServer,
                      IConsoleProducer consoleProducer,
                      IBufferManager bufferManager,
                      IParserManager parserManager,
                      IRabbitMQProducer rabbitMQProducer)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _output = configuration.GetSection("ConfigurationString").GetSection("Output").Value;
            _udpServer = udpServer;
            _consoleProducer = consoleProducer;
            _bufferManager = bufferManager;
            _parserManager = parserManager;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public Task StartServer()
        {
            var typeOfReciver = Enum.Parse<TypeOfReceiver>(ParserFactory.GetSTRING(_input, "protocol="));
            var typeOfProducer = Enum.Parse<TypeOfProducer>(ParserFactory.GetSTRING(_output, "protocol="));

            switch (typeOfReciver)
            {
                case TypeOfReceiver.udp:
                    new Task(() => ReciverRun(_udpServer, new TcpUdpReceiverParams(_input, Parser))).Start();
                    break;
                default:
                throw new ArgumentException($"Invalid incomming type of input protocol: {typeOfReciver}.");
            }

            switch (typeOfProducer)
            {
                case TypeOfProducer.console:
                    new Task(() => ProducerRun(_consoleProducer, new ConsoleProducerParams(Producer))).Start();
                    break;
                case TypeOfProducer.icmp:
                    new Task(() => ProducerRun(_rabbitMQProducer, new RabbitMQProducerParams(_output, Producer))).Start();
                    break;
                default:
                    throw new ArgumentException($"Invalid incomming type of output protocol: {typeOfProducer}.");
            }

            return Task.CompletedTask;
        }

        private void ReciverRun<T, K>(T reciver, K parameters) where T : IReciver<K> where K : ReceiverParameters
            => reciver.Run(parameters);

        private void ProducerRun<T, K>(T producer, K parameters) where T : IProducer<K> where K : ProducerParameters
            => producer.Run(parameters);

        private void Parser(string message)
            => _parserManager.ParseLogAndBufferize(message);

        private BsonDocument Producer()
            => _bufferManager.PullMessage();

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}