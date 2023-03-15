using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfProducers
{
    public class RabbitMQProducerParams : ProducerParameters
    {
        readonly IPAddress _ipAddress;
        readonly int _port;
        readonly string _userName;
        readonly string _password;
        readonly string _queue;

        public RabbitMQProducerParams(IConfiguration configuration, ProducerDelegate producer) 
            : base(producer)
        {
            var output = configuration.GetSection("ConfigurationString").GetSection("Output").Value;
            _ipAddress = ParserFactory.GetIP(output, "dstip=");
            _port = ParserFactory.GetINT(output, "dstport=");
            _userName = ParserFactory.GetSTRING(output, "username=");
            _password = ParserFactory.GetSTRING(output, "password=");
            _queue = ParserFactory.GetSTRING(output, "queue=");
        }

        public IPAddress IpAddress { get => _ipAddress; }
        public int Port { get => _port; }
        public string Username { get => _userName; }
        public string Password { get => _password; }
        public string Queue { get => _queue; }
    }
}