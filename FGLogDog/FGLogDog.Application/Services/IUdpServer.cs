using System;
using System.Net;
using System.Threading.Tasks;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal interface IUdpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port, int buferSize);
    }
}