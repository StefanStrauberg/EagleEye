using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Models
{
    public abstract class Parameters
    {
        private readonly ParserDelegate _parser;

        public Parameters(ParserDelegate parser)
            => _parser = parser;

        public ParserDelegate parse { get => _parser; }
    }
}