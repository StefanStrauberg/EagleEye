using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts.Server
{
    /// <summary>
    /// Interface represent the enum type of server and reciver
    /// </summary>
    public interface ITypeOfComponentsServer
    {
        /// <summary>
        /// Get enum type of receiver
        /// </summary>
        /// <returns></returns>
        TypeOfReceiver GetTypeOfReceiver();
        /// <summary>
        /// Get enum type of producer
        /// </summary>
        /// <returns></returns>
        TypeOfProducer GetTypeOfProducer();
    }
}