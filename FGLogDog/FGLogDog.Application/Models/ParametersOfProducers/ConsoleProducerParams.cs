using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfProducers
{
    public class ConsoleProducerParams : ProducerParameters
    {
        public ConsoleProducerParams(ProducerDelegate producer)
            : base(producer)
        {
        }
    }
}
