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

        public TcpUdpReceiverParams(string configuration, ParserDelegate parser)
            : base(parser)
        {
            _ipAddress = ParserFactory.GetIP(configuration, "srcip=");
            _port = ParserFactory.GetINT(configuration, "srcport=");
        }

        public IPAddress ipAddress { get => _ipAddress; }
        public int port { get => _port; }
    }
}