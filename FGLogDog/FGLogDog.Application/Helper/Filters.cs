using FGLogDog.Application.Helper;
using Microsoft.Extensions.Configuration;

namespace FGLogDog.FGLogDog.Application.Helper
{
    public class ConfigurationFilters : IConfigurationFilters
    {
        private readonly string[] _filters;
        private readonly string[] _patterns;

        public ConfigurationFilters(IConfiguration configuration)
        {
            _filters = configuration.GetSection("ConfigurationString").GetSection("Filter").Value.Split(" ");
            _patterns = ParserFactory.ReplacePatterns(configuration.GetSection("ConfigurationString").GetSection("Filter").Value);
        }

        public string[] Filters { get => _filters; }
        public string[] Patterns { get => _patterns; }
    }
}