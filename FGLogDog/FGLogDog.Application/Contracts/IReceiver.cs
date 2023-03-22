using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Contracts
{
    public interface IReceiver : IDisposable
    {
        void Run();
    }
}