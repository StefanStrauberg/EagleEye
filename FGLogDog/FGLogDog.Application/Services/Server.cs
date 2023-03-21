using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Contracts.Server;
using FGLogDog.Application.Contracts.TemporaryBuffer;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class Server : BackgroundService
    {
        readonly ITypeOfComponentsServer _typeOfServer;
        readonly IBufferRepository _bufferRepository;
        readonly IUDPReceiver _udpReceiver;
        readonly ITCPReceiver _tcpReceiver;
        readonly IRabbitMQProducer _rabbitMQProducer;

        public Server(ITypeOfComponentsServer typeOfServer,
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
                TypeOfReceiver.udp => ReceiverStart(_udpReceiver, stoppingToken),
                TypeOfReceiver.tcp => ReceiverStart(_tcpReceiver, stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of input protocol"),
            };

            _ = _typeOfServer.GetTypeOfProducer() switch
            {
                TypeOfProducer.amqp => ProducerStart(_rabbitMQProducer, stoppingToken),
                _ => throw new ArgumentException("Invalid incomming type of output protocol"),
            };
            
            return Task.CompletedTask;
        }

        Task ReceiverStart(IReceiver receiver, CancellationToken stoppingToken)
            => Task.Run(() => receiver.Run(), stoppingToken);
        
        Task ProducerStart(IProducer producer, CancellationToken stoppingToken)
            => Task.Run(() => producer.Run(), stoppingToken);
    }
}