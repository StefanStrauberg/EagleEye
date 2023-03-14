using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts.Reciver
{
    public interface IUdpServer : IReciver<TcpUdpReciverParams>
    {
    }
}