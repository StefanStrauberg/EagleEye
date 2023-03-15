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
        readonly IConfiguration _configuration;
        readonly IUdPReceiver _udpReceiver;
        readonly IBufferManager _bufferManager;
        readonly IParserManager _parserManager;
        readonly IRabbitMQProducer _rabbitMQProducer;
        readonly TypeOfReceiver _typeOfReciver;
        readonly TypeOfProducer _typeOfProducer;

        public Server(IConfiguration configuration,
                      IUdPReceiver udpReceiver,
                      IBufferManager bufferManager,
                      IParserManager parserManager,
                      IRabbitMQProducer rabbitMQProducer)
        {
            var _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            var _output = configuration.GetSection("ConfigurationString").GetSection("Output").Value;
            _typeOfReciver = Enum.Parse<TypeOfReceiver>(ParserFactory.GetSTRING(_input, "protocol="));
            _typeOfProducer = Enum.Parse<TypeOfProducer>(ParserFactory.GetSTRING(_output, "protocol="));
            _configuration = configuration;
            _udpReceiver = udpReceiver;
            _bufferManager = bufferManager;
            _parserManager = parserManager;
            _rabbitMQProducer = rabbitMQProducer;
        }

        public Task StartServer()
        {
            switch (_typeOfReciver)
            {
                case TypeOfReceiver.udp:
                    Task.Run(() => ReceiverRun(_udpReceiver, new UdpReceiverParams(_configuration, Parser)));
                    break;
                default:
                throw new ArgumentException("Invalid incomming type of input protocol");
            }

            switch (_typeOfProducer)
            {
                case TypeOfProducer.amqp:
                    Task.Run(() => ProducerRun(_rabbitMQProducer, new RabbitMQProducerParams(_configuration, Producer)));
                    break;
                default:
                    throw new ArgumentException("Invalid incomming type of output protocol");
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