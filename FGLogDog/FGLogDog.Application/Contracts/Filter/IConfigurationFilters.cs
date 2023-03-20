using FGLogDog.Application.Models;

namespace FGLogDog.Application.Contracts.Filter
{
    /// <summary>
    /// General server parameters for filtering incoming data
    /// </summary>
    internal interface IConfigurationFilters
    {
        /// <summary>
        /// Common keys of filter
        /// </summary>
        /// <value></value>
        string[] FilterKeys { get; }
        /// <summary>
        /// Common filter patterns of filter
        /// </summary>
        /// <value></value>
        ParserTypes[] FilterPatterns { get; }
        /// <summary>
        /// Common searchable substrings of filter
        /// </summary>
        /// <value></value>
        string[] SearchableSubStrings { get; }
    }
}