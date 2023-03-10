using FGLogDog.Application.Contracts;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FGLogDog.Console.Producer
{
    internal class ConsoleProducer : IConsoleProducer
    {
        private readonly ILogger<ConsoleProducer> _logger;

        public ConsoleProducer(ILogger<ConsoleProducer> logger)
            => _logger = logger;

        public Task Run(ConsoleProducerParams parameters)
        {
            throw new System.NotImplementedException();
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}
