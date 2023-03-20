using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Buffer;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Contracts.Server;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : BackgroundService
    {
        readonly ITypeOfServer _typeOfServer;
        readonly IBufferRepository _bufferRepository;
        readonly IUDPReceiver _udpReceiver;
        readonly ITCPReceiver _tcpReceiver;
        readonly IRabbitMQProducer _rabbitMQProducer;

        public Server(ITypeOfServer typeOfServer,
                      IUDPReceiver udpReceiver,
                      IRabbitMQProducer rabbitMQProducer,
                      ITCPReceiver tcpReceiver,
                      IBufferRepository bufferRepository)
        {
            _typeOfServer = typeOfServer;
            _udpReceiver = udpReceiver;
            _rabbitMQProducer = rabbitMQProducer;
            _tcpReceiver = tcpReceiver;
            _bufferRepository = bufferRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            _ = _typeOfServer.GetTypeOfReceiver() switch
            {
                TypeOfReceiver.udp => Task.Run(() => ReceiverRun(_udpReceiver), stoppingToken),
                TypeOfReceiver.tcp => Task.Run(() => ReceiverRun(_tcpReceiver), stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of input protocol"),
            };
            _ = _typeOfServer.GetTypeOfProducer() switch
            {
                TypeOfProducer.amqp => Task.Run(() => ProducerRun(_rabbitMQProducer), stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of output protocol"),
            };
            
            return Task.CompletedTask;
        }

        void ReceiverRun<T>(T reciver) where T : IReceiver
            => reciver.Run();

        void ProducerRun<T>(T producer) where T : IProducer
            => producer.Run();
    }
}