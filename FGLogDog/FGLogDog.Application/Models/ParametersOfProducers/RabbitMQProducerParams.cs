using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.FGLogDog.Application.Models.ParametersOfProducers
{
    public class RabbitMQProducerParams : ProducerParameters
    {
        public RabbitMQProducerParams(ProducerDelegate producer) 
            : base(producer)
        {
        }
    }
}