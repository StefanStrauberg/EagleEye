using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Helper
{
    internal class ConfigurationFilters : IConfigurationFilters
    {
        readonly string[] _filterKeys;
        readonly string[] _filterPatterns;
        readonly string[] _searchSubStrings;

        public ConfigurationFilters(IConfiguration configuration)
        {
            _filterKeys = ParserFactory.GetKeysFromConfigurationFilters(configuration.GetSection("ConfigurationString")
                                                                                     .GetSection("Filter")
                                                                                     .Value);
            _filterPatterns = ParserFactory.GetPatternsFromConfigurationFilters(configuration.GetSection("ConfigurationString")
                                                                                             .GetSection("Filter")
                                                                                             .Value);
            _searchSubStrings = ParserFactory.ReplaceReadablePatterns(configuration.GetSection("ConfigurationString")
                                                                           .GetSection("Filter")
                                                                           .Value);
        }

        public string[] FilterKeys { get => _filterKeys; }
        public string[] FilterPatterns { get => _filterPatterns; }
        public string[] Patterns { get => _searchSubStrings; }
    }
}