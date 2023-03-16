using FGLogDog.Application.Helper;

namespace FGLogDog.FGLogDog.Application.Helper
{
    internal interface IConfigurationFilters
    {
        public string[] FilterKeys { get; }
        public ParserTypes[] FilterPatterns { get; }
        public string[] SearchableSubStrings { get; }
    }
}