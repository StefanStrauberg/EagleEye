using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;

namespace FGLogDog.Application.Contracts.Reciver
{
    public interface IUdpServer : IReciver<TcpUdpReceiverParams>
    {
    }
}