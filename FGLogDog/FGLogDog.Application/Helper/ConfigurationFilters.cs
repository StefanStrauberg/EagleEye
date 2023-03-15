using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Helper
{
    internal class ConfigurationFilters : IConfigurationFilters
    {
        readonly string[] _filterKeys;
        readonly string[] _filterPatterns;
        readonly string[] _searchableSubStrings;

        public ConfigurationFilters(IConfiguration configuration)
        {
            string filter = configuration.GetSection("ConfigurationString").GetSection("Filter").Value;
            _filterKeys = ParserFactory.GetKeysFromConfigurationFilters(filter);
            _filterPatterns = ParserFactory.GetPatternsFromConfigurationFilters(filter);
            _searchableSubStrings = ParserFactory.ReplaceReadablePatterns(filter);
        }

        public string[] FilterKeys { get => _filterKeys; }
        public string[] FilterPatterns { get => _filterPatterns; }
        public string[] SearchableSubStrings { get => _searchableSubStrings; }
    }
}