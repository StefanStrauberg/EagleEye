using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.FGLogDog.Application.Models.ParametersOfProducers;
using MongoDB.Bson;
using System;

namespace FGLogDog.Console.Producer
{
    internal class ConsoleProducer : IConsoleProducer
    {
        readonly IAppLogger<ConsoleProducer> _logger;

        public ConsoleProducer(IAppLogger<ConsoleProducer> logger)
            => _logger = logger;

        void IProducer<ConsoleProducerParams>.Run(ConsoleProducerParams parameters)
        {
            _logger.LogInformation($"LogDog started console producer");

            try
            {
                while (true)
                {
                    var message = parameters.getMessage();
                    System.Console.WriteLine(message.ToString());
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
