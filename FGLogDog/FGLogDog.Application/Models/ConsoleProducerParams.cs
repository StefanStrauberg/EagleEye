using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public class ConsoleProducerParams : ProducerParameters
    {
        public ConsoleProducerParams(ProducerDelegate producer)
            : base(producer)
        {
        }
    }
}
