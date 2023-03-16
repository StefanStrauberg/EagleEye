using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;
using FGLogDog.UDP.Receiver.Config;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FGLogDog.UDP.Receiver
{
    internal class UdpServer : IUdPReceiver
    {
        readonly IReceiverConfiguration _receiverConfiguration;
        readonly IAppLogger<UdpServer> _logger;
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;

        public UdpServer(IAppLogger<UdpServer> logger, IReceiverConfiguration receiverConfiguration)
        {
            _logger = logger;
            _receiverConfiguration = receiverConfiguration;
        }

        void IReceiver<UdpReceiverParams>.Run(UdpReceiverParams parameters)
        {
            Initialize();
            try
            {
                _ = Task.Run(async () =>
                {
                    SocketReceiveMessageFromResult res;
                    while (true) 
                    {
                        res = await _socket.ReceiveMessageFromAsync(_bufferRecvSegment,
                                                                    SocketFlags.None,
                                                                    _endPoint);
                        var message = Encoding.UTF8.GetString(_bufferRecv,
                                                             0,
                                                             res.ReceivedBytes);
                        //if (parameters.IsCommonCheck)
                        //    if (message.Contains(parameters.Common))
                        //        parameters.parse(message);
                        //    else
                        //        continue;
                        //else
                            parameters.parse(message);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(GetExceptionMessages(ex, nameof(UdpServer)));
            }
        }

        void Initialize()
        {
            try
            {
                _bufferRecv = new byte[4096];
                _bufferRecvSegment = new(_bufferRecv);
                _endPoint = new IPEndPoint(IPAddress.Parse(_receiverConfiguration.IpAddress), _receiverConfiguration.Port);
                _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
                _socket.Bind(_endPoint);
                _logger.LogInformation($"LogDog started UDP reciver on {_receiverConfiguration.IpAddress}:{_receiverConfiguration.Port}");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(GetExceptionMessages(ex, nameof(UdpServer)));
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
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
}