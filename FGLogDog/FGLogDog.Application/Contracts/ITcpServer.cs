using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts
{
    public interface ITcpServer : IReciver<TcpUdpReciverParams>
    {
    }
}