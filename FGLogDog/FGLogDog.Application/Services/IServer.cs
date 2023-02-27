using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    public interface IServer : IDisposable
    {
        Task StartServer();
    }
}