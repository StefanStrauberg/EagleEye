using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;

namespace FGLogDog.Application.Contracts
{
    public interface IUdpServer : IDisposable
    {
        Task Start(IPAddress iPAddress, int port, IMediator mediator);
    }
}