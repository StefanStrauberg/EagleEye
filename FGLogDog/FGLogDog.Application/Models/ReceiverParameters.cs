using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public abstract class ReceiverParameters
    {
        private readonly ParserDelegate _parser;

        public ReceiverParameters(ParserDelegate parser)
            => _parser = parser;

        public ParserDelegate parse { get => _parser; }
    }
}