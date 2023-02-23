using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Services
{
    public interface IUdpServer : IDisposable
    {
        Task ServerStart();
    }
}