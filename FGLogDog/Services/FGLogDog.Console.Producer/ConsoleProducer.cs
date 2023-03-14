using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Models;
using System;
using System.Threading.Tasks;

namespace FGLogDog.Console.Producer
{
    internal class ConsoleProducer : IConsoleProducer
    {
        readonly IAppLogger<ConsoleProducer> _logger;

        public ConsoleProducer(IAppLogger<ConsoleProducer> logger)
            => _logger = logger;

        public async Task Run(ConsoleProducerParams parameters)
        {
            _logger.LogInformation($"LogDog started console producer");
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
                _logger.LogWarning($"LogDog producer stoped.\n{ex.Message}");
            }
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}
