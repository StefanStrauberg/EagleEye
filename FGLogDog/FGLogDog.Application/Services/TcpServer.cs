using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FGLogDog.FGLogDog.Application.Services
{
    internal class TcpServer : ITcpServer
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        public TcpServer(ILogger<TcpServer> logger,
                         IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Start(IPAddress ipAddress, int port)
        {

            try
            {
                await Task.Run(() => System.Console.WriteLine("Test from tcp"));
            }
            catch (Exception ex)
            {
                _logger.LogError("{0} LogDog server stoped.\n{1}", DateTime.Now, ex.Message);
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}