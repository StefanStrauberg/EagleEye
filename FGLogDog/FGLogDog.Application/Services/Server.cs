using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Buffer = FGLogDog.Application.Models.Buffer;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : BackgroundService
    {
        readonly IUDPReceiver _udpReceiver;
        readonly ITCPReceiver _tcpReceiver;
        readonly IRabbitMQProducer _rabbitMQProducer;
        readonly TypeOfReceiver _typeOfReciver;
        readonly TypeOfProducer _typeOfProducer;

        public Server(IConfiguration configuration,
                      IUDPReceiver udpReceiver,
                      IRabbitMQProducer rabbitMQProducer,
                      IConfigurationFilters filters,
                      ITCPReceiver tcpReceiver)
        {
            ParserFactory.InitFilter(filters);
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
            _rabbitMQProducer = rabbitMQProducer;
            _tcpReceiver = tcpReceiver;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _ = _typeOfReciver switch
            {
                TypeOfReceiver.udp => Task.Run(() => ReceiverRun(_udpReceiver), stoppingToken),
                TypeOfReceiver.tcp => Task.Run(() => ReceiverRun(_tcpReceiver), stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of input protocol"),
            };
            _ = _typeOfProducer switch
            {
                TypeOfProducer.amqp => Task.Run(() => ProducerRun(_rabbitMQProducer), stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of output protocol"),
            };
            return Task.CompletedTask;
        }

        static void ReceiverRun<T>(T reciver) where T : IReceiver
            => reciver.Run(new ReceiverParameters((byte[] bytes) => Buffer.buffer.Add(bytes)));

        void ProducerRun<T>(T producer) where T : IProducer
            => producer.Run(new ProducerParameters(() => ParserFactory.GetMessage(Buffer.buffer.Take())));
    }
}