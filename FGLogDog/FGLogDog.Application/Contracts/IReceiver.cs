using FGLogDog.Application.Models;
using System;

namespace FGLogDog.Application.Contracts
{
    public interface IReceiver<in T> : IDisposable where T : ReceiverParameters
    {
        void Run(T parameters);
    }
}