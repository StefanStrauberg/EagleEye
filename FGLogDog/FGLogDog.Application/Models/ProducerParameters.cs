using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public class ProducerParameters
    {
        readonly ProducerDelegate _producer;

        public ProducerParameters(ProducerDelegate producer)
            => _producer = producer;

        public ProducerDelegate getMessage { get => _producer; }
    }
}