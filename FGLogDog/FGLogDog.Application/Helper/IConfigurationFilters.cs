namespace FGLogDog.FGLogDog.Application.Helper
{
    internal interface IConfigurationFilters
    {
        public string[] FilterKeys { get; }
        public string[] FilterPatterns { get; }
        public string[] Patterns { get; }
    }
}