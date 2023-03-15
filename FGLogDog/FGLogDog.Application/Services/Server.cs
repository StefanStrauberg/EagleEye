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
        readonly string _common;
        readonly IUdPReceiver _udpReceiver;
        readonly IBufferManager _bufferManager;
        readonly IParserManager _parserManager;
        readonly IRabbitMQProducer _rabbitMQProducer;

        public Server(IConfiguration configuration,
                      IUdPReceiver udpReceiver,
                      IBufferManager bufferManager,
                      IParserManager parserManager,
                      IRabbitMQProducer rabbitMQProducer)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _output = configuration.GetSection("ConfigurationString").GetSection("Output").Value;
            _common = configuration.GetSection("ConfigurationString").GetSection("Common").Value;
            _udpReceiver = udpReceiver;
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
                    Task.Run(() => ReceiverRun(_udpReceiver, new TcpUdpReceiverParams(_input, _common, Parser)));
                    break;
                default:
                throw new ArgumentException($"Invalid incomming type of input protocol: {typeOfReciver}.");
            }

            switch (typeOfProducer)
            {
                case TypeOfProducer.amqp:
                    Task.Run(() => ProducerRun(_rabbitMQProducer, new RabbitMQProducerParams(_output, Producer)));
                    break;
                default:
                    throw new ArgumentException($"Invalid incomming type of output protocol: {typeOfProducer}.");
            }

            return Task.CompletedTask;
        }

        static void ReceiverRun<T, K>(T reciver, K parameters) where T : IReceiver<K> where K : ReceiverParameters
            => reciver.Run(parameters);

        static void ProducerRun<T, K>(T producer, K parameters) where T : IProducer<K> where K : ProducerParameters
            => producer.Run(parameters);

        void Parser(string message)
            => _parserManager.ParseLogAndBufferize(message);

        BsonDocument Producer()
            => _bufferManager.PullMessage();

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}