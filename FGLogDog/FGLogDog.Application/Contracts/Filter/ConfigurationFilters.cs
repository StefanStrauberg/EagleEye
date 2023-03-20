using FGLogDog.Application.Helpers;
using FGLogDog.Application.Models;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.Application.Contracts.Filter
{
    internal class ConfigurationFilters : IConfigurationFilters
    {
        readonly string[] _filterKeys;
        readonly ParserTypes[] _filterPatterns;
        readonly string[] _searchableSubStrings;

        public ConfigurationFilters(IConfiguration configuration)
        {
            string filter = configuration.GetSection("ServiceConfiguration").GetSection("Filter").Value;
            _filterKeys = RegexHelper.GetKeysFromConfigurationFilters(filter);
            _filterPatterns = RegexHelper.GetPatternsFromConfigurationFilters(filter);
            _searchableSubStrings = RegexHelper.ReplaceReadablePatterns(filter);
        }

        string[] IConfigurationFilters.FilterKeys { get => _filterKeys; }
        ParserTypes[] IConfigurationFilters.FilterPatterns { get => _filterPatterns; }
        string[] IConfigurationFilters.SearchableSubStrings { get => _searchableSubStrings; }
    }
}