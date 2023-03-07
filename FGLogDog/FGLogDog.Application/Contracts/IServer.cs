using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Contracts
{
    public interface IServer : IDisposable
    {
        Task StartServer();
    }
}