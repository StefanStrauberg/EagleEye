using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public class ReceiverParameters
    {
        readonly ParserDelegate _parser;

        public ReceiverParameters(ParserDelegate parser)
            => _parser = parser;

        public ParserDelegate Parse { get => _parser; }
    }
}