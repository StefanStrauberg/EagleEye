using System;

namespace FGLogDog.Application.Contracts
{
    public interface IReceiver : IDisposable
    {
        void Run();
    }
}