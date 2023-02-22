using System;
using System.Threading.Tasks;
using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.Application.Services
{
    public class UdpReceiver : IUdpReceiver
    {
        private readonly string _input;
        private readonly string _srcip;
        private readonly string _srcport;

        public UdpReceiver(IConfiguration configuration)
        {
            _input = configuration.GetSection("ConfigurationString").GetSection("Input").Value;
            ParserFactory.SearchSubstring(_input, "srcip=", ParserTypes.IP, out _srcip);
            ParserFactory.SearchSubstring(_input, "srcport=", ParserTypes.INT, out _srcport);
        }

        public Task Start()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
            => GC.SuppressFinalize(this);
    }
}