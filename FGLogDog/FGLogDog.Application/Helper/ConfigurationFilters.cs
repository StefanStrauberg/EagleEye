using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Helper
{
    internal class ConfigurationFilters : IConfigurationFilters
    {
        private readonly string[] _filterKeys;
        private readonly string[] _filterPatterns;
        private readonly string[] _patterns;

        public ConfigurationFilters(IConfiguration configuration)
        {
            _filterKeys = ParserFactory.GetKeysFromConfigurationFilters(configuration.GetSection("ConfigurationString")
                                                                                     .GetSection("Filter")
                                                                                     .Value);
            _filterPatterns = ParserFactory.GetPatternsFromConfigurationFilters(configuration.GetSection("ConfigurationString")
                                                                                             .GetSection("Filter")
                                                                                             .Value);
            _patterns = ParserFactory.ReplaceReadablePatterns(configuration.GetSection("ConfigurationString")
                                                                           .GetSection("Filter")
                                                                           .Value);
        }

        public string[] FilterKeys { get => _filterKeys; }
        public string[] FilterPatterns { get => _filterPatterns; }
        public string[] Patterns { get => _patterns; }
    }
}