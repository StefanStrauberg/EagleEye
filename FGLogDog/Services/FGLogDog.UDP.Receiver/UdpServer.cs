using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.Application.Models;
using FGLogDog.UDP.Receiver.Config;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FGLogDog.UDP.Receiver
{
    internal class UDPServer : IUDPReceiver
    {
        readonly IReceiverConfiguration _receiverConfiguration;
        readonly IAppLogger<UDPServer> _logger;
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;
        bool _isBrockenInitialize;

        public UDPServer(IAppLogger<UDPServer> logger,
                         IReceiverConfiguration receiverConfiguration)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
        }

        void IReceiver.Run(ReceiverParameters parameters)
        {
            Initialize();
            if (!_isBrockenInitialize)
            {
                try
                {
                    _ = Task.Run(async () =>
                    {
                        SocketReceiveMessageFromResult res;
                        while (true)
                        {
                            res = await _socket.ReceiveMessageFromAsync(_bufferRecvSegment, _endPoint);
                            parameters.PushToBuffer(_bufferRecv);
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
                _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
                _socket.Bind(_endPoint);
                _logger.LogInformation($"LogDog started UDP reciver on {_receiverConfiguration.IpAddress}:{_receiverConfiguration.Port}");
            }
            catch (Exception ex)
            {
                _logger.LogError(GetExceptionMessages(ex, nameof(UDPServer)));
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