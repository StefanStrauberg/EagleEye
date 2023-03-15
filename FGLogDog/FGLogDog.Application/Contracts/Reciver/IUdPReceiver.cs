using FGLogDog.FGLogDog.Application.Models.ParametersOfReceivers;

namespace FGLogDog.Application.Contracts.Reciver
{
    public interface IUdPReceiver : IReceiver<TcpUdpReceiverParams>
    {
    }
}