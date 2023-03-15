using FGLogDog.FGLogDog.Application.Models.ParametersOfProducers;

namespace FGLogDog.Application.Contracts.Producer
{
    public interface IRabbitMQProducer : IProducer<RabbitMQProducerParams>
    {
    }
}
