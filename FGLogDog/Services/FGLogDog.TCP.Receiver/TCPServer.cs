using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Filter;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Contracts.TemporaryBuffer;
using FGLogDog.Application.Errors;
using FGLogDog.TCP.Receiver.Config;

namespace FGLogDog.TCP.Receiver
{
    internal class TCPServer : ITCPReceiver
    {
        readonly IReceiverConfiguration _receiverConfiguration;
        readonly IAppLogger<TCPServer> _logger;
        readonly ICommonFilter _commonFilter;
        readonly IBufferRepository _bufferRepository;
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;

        public TCPServer(IAppLogger<TCPServer> logger,
                         IReceiverConfiguration receiverConfiguration,
                         IBufferRepository bufferRepository,
                         ICommonFilter commonFilter)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
            _bufferRepository = bufferRepository;
            _commonFilter = commonFilter;
        }

        async void IReceiver.Run()
        {
            try
            {
                Initialize();
                Socket handler = _socket.Accept();
                while(true)
                {
                    int bytesRec = await handler.ReceiveAsync(_bufferRecvSegment);
                    if (_commonFilter.Contain(_bufferRecv))
                        _bufferRepository.PushToBuffer(_bufferRecv);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ConvertExceptionMessage.GetExceptionMessages(ex, nameof(TCPServer)));
            }
            finally
            {
                (this as IDisposable).Dispose();
            }
        }

        void Initialize()
        {
            _bufferRecv = new byte[2048];
            _bufferRecvSegment = new(_bufferRecv);
            _endPoint = new IPEndPoint(IPAddress.Parse(_receiverConfiguration.IpAddress), _receiverConfiguration.Port);
            _socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _socket.Bind(_endPoint);
            _socket.Listen(1);
            _logger.LogInformation($"LogDog started TCP reciver on {_receiverConfiguration.IpAddress}:{_receiverConfiguration.Port}");
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}