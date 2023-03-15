using System.Net;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers
{
    public class TcpUdpReceiverParams : ReceiverParameters
    {
        readonly IPAddress _ipAddress;
        readonly int _port;
        readonly string _common;

        public TcpUdpReceiverParams(string configuration, string common, ParserDelegate parser)
            : base(parser)
        {
            _ipAddress = ParserFactory.GetIP(configuration, "srcip=");
            _port = ParserFactory.GetINT(configuration, "srcport=");
            _common = common;
        }

        public IPAddress IpAddress { get => _ipAddress; }
        public int Port { get => _port; }
        public string Common { get => _common; }
        public bool IsCommonCheck { get => _common.Length > 0; }
    }
}