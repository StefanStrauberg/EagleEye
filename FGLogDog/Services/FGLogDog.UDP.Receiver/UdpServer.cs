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
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;

        public UDPServer(IAppLogger<UDPServer> logger,
                         IReceiverConfiguration receiverConfiguration,
                         IBufferRepository bufferRepository)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
            _bufferRepository = bufferRepository;
        }

        async void IReceiver.Run()
        {
            try
            {
                Initialize();
                SocketReceiveMessageFromResult res;
                while (true)
                {
                    res = await _socket.ReceiveMessageFromAsync(_bufferRecvSegment, _endPoint);
                    _bufferRepository.PushToBuffer(_bufferRecv);
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

        void Initialize()
        {
            _bufferRecv = new byte[_receiverConfiguration.SizeOfBuffer];
            _bufferRecvSegment = new(_bufferRecv);
            _endPoint = new IPEndPoint(IPAddress.Parse(_receiverConfiguration.IpAddress), _receiverConfiguration.Port);
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket.Bind(_endPoint);
            _logger.LogInformation($"LogDog started UDP reciver on "+ 
                                   $"{_receiverConfiguration.IpAddress}:{_receiverConfiguration.Port} "+
                                   $"with BufferSize:{_receiverConfiguration.SizeOfBuffer}");
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}