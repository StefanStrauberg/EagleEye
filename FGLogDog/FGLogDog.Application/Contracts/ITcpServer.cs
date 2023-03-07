using System.Net;
using System;
using System.Threading.Tasks;
using MediatR;

namespace FGLogDog.Application.Contracts
{
    public interface ITcpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port, IMediator mediator);
    }
}