using FGLogDog.Application.Models;
using System;
using System.Threading.Tasks;

namespace FGLogDog.Application.Contracts
{
    public interface IReciver<in T> : IDisposable where T : ReciverParameters
    {
        Task Run(T parameters);
    }
}