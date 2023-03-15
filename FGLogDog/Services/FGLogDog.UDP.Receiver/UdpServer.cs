using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FGLogDog.UDP.Receiver
{
    internal class UdpServer : IUdPReceiver
    {   
        readonly IAppLogger<UdpServer> _logger;
        Socket _socket;
        EndPoint _endPoint;
        byte[] _bufferRecv;
        ArraySegment<byte> _bufferRecvSegment;

        public UdpServer(IAppLogger<UdpServer> logger)
            => _logger = logger;

        void IReceiver<TcpUdpReceiverParams>.Run(TcpUdpReceiverParams parameters)
        {
            try
            {
                Initialize(parameters.IpAddress, parameters.Port);
                _logger.LogInformation($"LogDog started UDP reciver on {parameters.IpAddress}:{parameters.Port}");
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
                        if (parameters.IsCommonCheck)
                            if (message.Contains(parameters.Common))
                                parameters.parse(message);
                            else
                                continue;
                        else
                            parameters.parse(message);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"LogDog reciver stoped.\n{ex.Message}");
            }
        }

        void Initialize(IPAddress iPAddress, int port)
        {
            _bufferRecv = new byte[4096];
            _bufferRecvSegment = new(_bufferRecv);
            _endPoint = new IPEndPoint(iPAddress, port);
            _socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            _socket.Bind(_endPoint);
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}