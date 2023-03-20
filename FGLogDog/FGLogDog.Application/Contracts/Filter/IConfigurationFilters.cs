using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts.Filter
{
    /// <summary>
    /// Common parameters of server such as FilterKey, FilterPatterns, SearchableSubStrings
    /// </summary>
    internal interface IConfigurationFilters
    {
        /// <summary>
        /// Common keys of filter
        /// </summary>
        /// <value></value>
        string[] FilterKeys { get; }
        /// <summary>
        /// Common keys of filter patterns
        /// </summary>
        /// <value></value>
        ParserTypes[] FilterPatterns { get; }
        /// <summary>
        /// Common keys of searchable substrings
        /// </summary>
        /// <value></value>
        string[] SearchableSubStrings { get; }
    }
}