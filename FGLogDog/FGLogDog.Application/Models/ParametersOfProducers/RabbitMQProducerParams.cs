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

        public IPAddress ipAddress { get => _ipAddress; }
        public int port { get => _port; }
        public string username { get => _userName; }
        public string password { get => _password; }
        public string queue { get => _queue; }
    }
}