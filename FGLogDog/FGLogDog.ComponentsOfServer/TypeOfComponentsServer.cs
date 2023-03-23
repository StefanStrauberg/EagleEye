using System;
using System.Linq;
using FGLogDog.Application.Contracts.Server;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.ComponentsOfServer
{
    internal class TypeOfComponentsServer : ITypeOfComponentsServer
    {
        readonly IConfiguration _configuration;

        public TypeOfComponentsServer(IConfiguration configuration)
            => _configuration = configuration;

        TypeOfProducer ITypeOfComponentsServer.GetTypeOfProducer()
            => Enum.Parse<TypeOfProducer>(_configuration.GetSection("ServiceConfiguration")
                                                        .GetSection("Producer")
                                                        .GetChildren()
                                                        .FirstOrDefault()
                                                        .Key);

        TypeOfReceiver ITypeOfComponentsServer.GetTypeOfReceiver()
            => Enum.Parse<TypeOfReceiver>(_configuration.GetSection("ServiceConfiguration")
                                                        .GetSection("Receiver")
                                                        .GetChildren()
                                                        .FirstOrDefault()
                                                        .Key);
    }
}