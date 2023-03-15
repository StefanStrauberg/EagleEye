using FGLogDog.Application.Helper;
using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using System.Net;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfProducers
{
    public class RabbitMQProducerParams : ProducerParameters
    {
        private readonly IPAddress _ipAddress;
        private readonly int _port;

        public RabbitMQProducerParams(string configuration, ProducerDelegate producer) 
            : base(producer)
        {
            _ipAddress = ParserFactory.GetIP(configuration, "dstip=");
            _port = ParserFactory.GetINT(configuration, "dstport=");
        }

        public IPAddress ipAddress { get => _ipAddress; }
        public int port { get => _port; }
    }
}