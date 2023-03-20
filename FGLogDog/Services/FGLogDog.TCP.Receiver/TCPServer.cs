using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Helper;
using FGLogDog.TCP.Receiver.Config;

namespace FGLogDog.TCP.Receiver
{
    internal class TCPServer : ITCPReceiver
    {
        readonly IReceiverConfiguration _receiverConfiguration;
        readonly IAppLogger<TCPServer> _logger;
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;

        public TCPServer(IAppLogger<TCPServer> logger,
                         IReceiverConfiguration receiverConfiguration)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
        }

        void IReceiver.Run(Action<byte[]> PushToBuffer)
        {
            try
            {
                Initialize();
                _ = Task.Run(async () =>
                {
                        Socket handler = _socket.Accept();
                        while(true)
                        {
                            int bytesRec = await handler.ReceiveAsync(_bufferRecvSegment);
                            PushToBuffer(_bufferRecv);
                        }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ConvertExceptionMessage.GetExceptionMessages(ex, nameof(TCPServer)));
            }
            finally
            {
                ((IDisposable)this).Dispose();
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