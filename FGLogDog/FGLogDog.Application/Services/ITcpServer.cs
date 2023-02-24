using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    public interface ITcpServer : IDisposable
    {
        Task ServerStart();
    }
}