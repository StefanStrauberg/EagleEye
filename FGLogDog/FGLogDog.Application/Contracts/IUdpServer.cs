using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts
{
    public interface IUdpServer : IReciver<TcpUdpReciverParams>
    {
    }
}