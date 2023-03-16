using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Commands;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : BackgroundService
    {
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
            _typeOfReciver = Enum.Parse<TypeOfReceiver>(configuration.GetSection("ServiceConfiguration")
                                                                     .GetSection("Receiver")
                                                                     .GetChildren()
                                                                     .FirstOrDefault()
                                                                     .Key);
            _typeOfProducer = Enum.Parse<TypeOfProducer>(configuration.GetSection("ServiceConfiguration")
                                                                      .GetSection("Producer")
                                                                      .GetChildren()
                                                                      .FirstOrDefault()
                                                                      .Key);
            _udpReceiver = udpReceiver;
            _bufferManager = bufferManager;
            _parserManager = parserManager;
            _rabbitMQProducer = rabbitMQProducer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _ = _typeOfReciver switch
            {
                TypeOfReceiver.udp => Task.Run(() => ReceiverRun(_udpReceiver, new ReceiverParameters(Parser)),
                                               stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of input protocol"),
            };
            _ = _typeOfProducer switch
            {
                TypeOfProducer.amqp => Task.Run(() => ProducerRun(_rabbitMQProducer, new ProducerParameters(Producer)),
                                                stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of output protocol"),
            };
            return Task.CompletedTask;
        }

        static void ReceiverRun<T>(T reciver, ReceiverParameters parameters) where T : IReceiver
            => reciver.Run(parameters);

        static void ProducerRun<T>(T producer, ProducerParameters parameters) where T : IProducer
            => producer.Run(parameters);

        void Parser(string message)
            => _parserManager.ParseLogAndBufferize(message);

        BsonDocument Producer()
            => _bufferManager.TakeMessage();
    }
}