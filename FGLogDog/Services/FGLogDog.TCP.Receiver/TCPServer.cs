using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Models;
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
        bool _isBrockenInitialize;

        public TCPServer(IAppLogger<TCPServer> logger,
                         IReceiverConfiguration receiverConfiguration)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
        }

        void IReceiver.Run(Action<byte[]> PushToBuffer)
        {
            Initialize();
            if (!_isBrockenInitialize)
            {
                try
                {
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
                    _logger.LogError(ex.ToString());
                }
            }
        }

        void Initialize()
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                _isBrockenInitialize = true;
            }
            finally
            {
                ((IDisposable)this).Dispose();
            }
        }

        static string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}