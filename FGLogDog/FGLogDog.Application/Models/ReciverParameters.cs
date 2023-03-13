using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public abstract class ReciverParameters
    {
        private readonly ParserDelegate _parser;

        public ReciverParameters(ParserDelegate parser)
            => _parser = parser;

        public ParserDelegate parse { get => _parser; }
    }
}