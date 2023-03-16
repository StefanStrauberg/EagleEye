using FGLogDog.Application.Models;
using System;

namespace FGLogDog.Application.Contracts
{
    public interface IProducer : IDisposable
    {
        void Run(ProducerParameters parameters);
    }
}
