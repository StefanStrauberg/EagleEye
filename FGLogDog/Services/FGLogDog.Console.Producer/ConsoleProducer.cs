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

        public async Task Run(ConsoleProducerParams parameters)
        {
            _logger.LogInformation($"{DateTime.Now} LogDog started Console producer");
            try
            {
                while (true)
                {
                    var message = await parameters.getMessage();
                    System.Console.WriteLine(message);
                } 
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.Now} LogDog producer stoped.\n{ex.Message}");
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}
