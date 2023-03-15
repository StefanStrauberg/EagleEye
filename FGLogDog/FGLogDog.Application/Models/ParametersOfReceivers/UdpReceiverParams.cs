using System.Net;
using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers
{
    public class UdpReceiverParams : ReceiverParameters
    {
        readonly IPAddress _ipAddress;
        readonly int _port;
        readonly string _common;

        public UdpReceiverParams(IConfiguration configuration, ParserDelegate parser)
            : base(parser)
        {
            var input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            _common = configuration.GetSection("ConfigurationString").GetSection("Common").Value;
            _ipAddress = ParserFactory.GetIP(input, "srcip=");
            _port = ParserFactory.GetINT(input, "srcport=");
        }

        public IPAddress IpAddress { get => _ipAddress; }
        public int Port { get => _port; }
        public string Common { get => _common; }
        public bool IsCommonCheck { get => _common.Length > 0; }
    }
}