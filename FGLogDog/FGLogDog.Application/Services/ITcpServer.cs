using System.Net;
using System;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal interface ITcpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port);
    }
}