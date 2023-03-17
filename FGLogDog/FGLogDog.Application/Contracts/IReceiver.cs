using FGLogDog.Application.Models;
using System;

namespace FGLogDog.Application.Contracts
{
    public interface IReceiver : IDisposable
    {
        void Run(Action<byte[]> PushToBuffer);
    }
}