namespace FGLogDog.FGLogDog.Application.Helper
{
    public interface IConfigurationFilters
    {
        public string[] Filters { get; }
        public string[] Patterns { get; }
    }
}