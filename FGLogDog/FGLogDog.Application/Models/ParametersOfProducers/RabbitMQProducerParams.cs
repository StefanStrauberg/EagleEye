using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
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

        public RabbitMQProducerParams(string configuration, ProducerDelegate producer) 
            : base(producer)
        {
            _ipAddress = ParserFactory.GetIP(configuration, "dstip=");
            _port = ParserFactory.GetINT(configuration, "dstport=");
            _userName = ParserFactory.GetSTRING(configuration, "username=");
            _password = ParserFactory.GetSTRING(configuration, "password=");
            _queue = ParserFactory.GetSTRING(configuration, "queue=");
        }

        public IPAddress IpAddress { get => _ipAddress; }
        public int Port { get => _port; }
        public string Username { get => _userName; }
        public string Password { get => _password; }
        public string Queue { get => _queue; }
    }
}