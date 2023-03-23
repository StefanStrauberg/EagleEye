using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Contracts.TemporaryBuffer;
using FGLogDog.Application.Errors;
using FGLogDog.UDP.Receiver.Config;
using System;
using System.Net;
using System.Net.Sockets;

namespace FGLogDog.UDP.Receiver
{
    internal class UDPServer : IUDPReceiver
    {
        readonly IReceiverConfiguration _receiverConfiguration;
        readonly IAppLogger<UDPServer> _logger;
        readonly IBufferRepository _bufferRepository;
        readonly IPEndPoint _endPoint;
        readonly UdpClient _udpClient;
        IPEndPoint _remoteIpEndPoint;

        public UDPServer(IAppLogger<UDPServer> logger,
                         IReceiverConfiguration receiverConfiguration,
                         IBufferRepository bufferRepository)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
            _bufferRepository = bufferRepository;
            _endPoint = new IPEndPoint(IPAddress.Parse(_receiverConfiguration.IpAddress), _receiverConfiguration.Port);
            _udpClient = _udpClient = new UdpClient(_endPoint);
        }

        void IReceiver.Run()
        {
            _logger.LogInformation($"LogDog started UDP reciver on " +
                                   $"{_receiverConfiguration.IpAddress}:{_receiverConfiguration.Port}");
            try
            {
                while (true)
                {
                    byte[] receiveBytes = _udpClient.Receive(ref _remoteIpEndPoint);
                    _bufferRepository.PushToBuffer(receiveBytes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ConvertExceptionMessage.GetExceptionMessages(ex, nameof(UDPServer)));
            }
            finally
            {
                (this as IDisposable).Dispose();
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}