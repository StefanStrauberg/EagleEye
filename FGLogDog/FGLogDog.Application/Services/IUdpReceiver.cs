using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Services
{
    public interface IUdpReceiver : IDisposable
    {
        Task Start();
    }
}