using System;

namespace FGLogDog.Application.Contracts
{
    public interface IProducer : IDisposable
    {
        void Run(Func<byte[]> PullFromBuffer);
    }
}
