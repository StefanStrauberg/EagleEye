using System;
using System.Threading.Tasks;
using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts
{
    public interface IReciver<in T> : IDisposable where T : ReciverParameters
    {
        Task Run(T parameters);
    }
}