using System;
using System.Linq;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.Application.Contracts.Server
{
    public class TypeOfServer : ITypeOfServer
    {
        readonly IConfiguration _configuration;

        public TypeOfServer(IConfiguration configuration)
            => _configuration = configuration;

        TypeOfProducer ITypeOfServer.GetTypeOfProducer()
            => Enum.Parse<TypeOfProducer>(_configuration.GetSection("ServiceConfiguration")
                                                        .GetSection("Producer")
                                                        .GetChildren()
                                                        .FirstOrDefault()
                                                        .Key);

        TypeOfReceiver ITypeOfServer.GetTypeOfReceiver()
            => Enum.Parse<TypeOfReceiver>(_configuration.GetSection("ServiceConfiguration")
                                                        .GetSection("Receiver")
                                                        .GetChildren()
                                                        .FirstOrDefault()
                                                        .Key);
    }
}