using FGLogDog.Application.Contracts;
using FGLogDog.Application.Contracts.Filter;
using FGLogDog.Application.Contracts.Logger;
using FGLogDog.Application.Contracts.Producer;
using FGLogDog.Application.Contracts.TemporaryBuffer;
using FGLogDog.Application.Errors;
using FGLogDog.RabbitMQ.Producer.Config;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace FGLogDog.RabbitMQ.Producer
{
    internal class RabbitMqProducer : IRabbitMQProducer
    {
        readonly IProducerConfiguration _producerConfiguration;
        readonly IAppLogger<RabbitMqProducer> _logger;
        readonly IParserFactory _parserFactory;
        readonly IBufferRepository _bufferRepository;
        IConnection _connection;
        IModel _channel;

        public RabbitMqProducer(IAppLogger<RabbitMqProducer> logger,
                                IProducerConfiguration producerConfiguration,
                                IParserFactory parserFactory,
                                IBufferRepository bufferRepository)
        {
            _logger = logger;
            _producerConfiguration = producerConfiguration;
            _parserFactory = parserFactory;
            _bufferRepository = bufferRepository;
        }

        void IProducer.Run()
        {
            try
            {
                Initialize();
                _ = Task.Run(() =>
                {
                    while (true)
                    {
                        var bytes = _bufferRepository.PullFromBuffer();
                        var body = _parserFactory.ParsingMessage(bytes);
                        if (body is null)
                            continue;
                        _channel.BasicPublish(exchange: "",
                                              routingKey: _producerConfiguration.Queue,
                                              basicProperties: null,
                                              body: body);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ConvertExceptionMessage.GetExceptionMessages(ex, nameof(RabbitMqProducer)));
            }
            finally
            {
                ((IDisposable)this).Dispose();
            }
        }

        void Initialize()
        {
            var factory = new ConnectionFactory
            {
                HostName = _producerConfiguration.IpAddress,
                Port = _producerConfiguration.Port,
                UserName = _producerConfiguration.UserName,
                Password = _producerConfiguration.Password
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _producerConfiguration.Queue,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            _logger.LogInformation($"LogDog started rabbitmq producer to {_producerConfiguration.IpAddress}:{_producerConfiguration.Port}");
        }

        void IDisposable.Dispose()
            => GC.SuppressFinalize(this);
    }
}