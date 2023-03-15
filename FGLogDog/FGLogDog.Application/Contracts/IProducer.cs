using FGLogDog.Application.Models;
using System;

namespace FGLogDog.Application.Contracts
{
    public interface IProducer<T> : IDisposable where T : ProducerParameters
    {
        void Run(T parameters);
    }
}
