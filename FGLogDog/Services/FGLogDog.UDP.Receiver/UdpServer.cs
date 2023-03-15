using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Reciver;
using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FGLogDog.UDP.Receiver
{
    internal class UdpServer : IUdPReceiver
    {   
        readonly IAppLogger<UdpServer> _logger;

        public UdpServer(IAppLogger<UdpServer> logger)
            => _logger = logger;

        void IReceiver<TcpUdpReceiverParams>.Run(TcpUdpReceiverParams parameters)
        {
            IPEndPoint ipPoint = new IPEndPoint(parameters.ipAddress, parameters.port);
            UdpClient udpClient = new UdpClient(ipPoint);
            IPEndPoint RemoteIpEndPoint = null;

            _logger.LogInformation($"LogDog started UDP reciver on {parameters.ipAddress}:{parameters.port}");

            try
            {
                while (true)
                {
                    byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string message = Encoding.UTF8.GetString(receiveBytes);
                    parameters.parse(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"LogDog reciver stoped.\n{ex.Message}");
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}