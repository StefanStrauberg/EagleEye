namespace FGLogDog.FGLogDog.Application.Helper
{
    public interface IFilters
    {
        public string[] Filter { get; }
        public string[] Patterns { get; }
    }
}