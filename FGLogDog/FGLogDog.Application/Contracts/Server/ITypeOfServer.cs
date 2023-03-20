using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts.Server
{
    internal interface ITypeOfServer
    {
        TypeOfReceiver GetTypeOfReceiver();
        TypeOfProducer GetTypeOfProducer();
    }
}