using System.Text;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Text.Json.Nodes;
using FGLogDog.FGLogDog.Application.Helper;

namespace FGLogDog.Application.Helper
{
    public static class ParserFactory
    {
        private const string _int = @"(\d+)";
        private const string _time = @"([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        private const string _date = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        private const string _string = @"([^""\\]*(?:\\.[^""\\]*)*)";
        private const string _guid = @"([({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?)";
        private const string _mac = @"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})";
        private const string _ip = @"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

        private static string GetPattern(ParserTypes type)
            => type switch
            {
                ParserTypes.INT => _int,
                ParserTypes.TIME => _time,
                ParserTypes.DATE => _date,
                ParserTypes.STRING => _string,
                ParserTypes.GUID => _guid,
                ParserTypes.MAC => _mac,
                ParserTypes.IP => _ip,
                _ => throw new ArgumentException("Invalid incoming data of ParserTypes.")
            };
        
        private static string GetMatch(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
                return matches.First().Value;
            return null;
        }

        private static object GetObjectByPattern(string inputString, ParserTypes pattern)
            => pattern switch
            {
                ParserTypes.DATE => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        if (DateOnly.TryParse(matches.First().Value, out DateOnly data))
                            return data;
                    return DateOnly.MinValue;
                },
                ParserTypes.TIME => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        if (TimeOnly.TryParse(matches.First().Value, out TimeOnly data))
                            return data;
                    return TimeOnly.MinValue;
                },
                ParserTypes.INT => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        if (Int32.TryParse(matches.First().Value, out int data))
                            return data;
                    return 0;
                },
                ParserTypes.GUID => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        if (Guid.TryParse(matches.First().Value, out Guid data))
                            return data;
                    return Guid.Empty;
                },
                ParserTypes.MAC => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        return matches.First().Value;
                    return null;
                },
                ParserTypes.IP => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        if (IPAddress.TryParse(matches.First().Value, out IPAddress data))
                            return data;
                    return null;
                },
                ParserTypes.STRING => () => 
                {
                    var matches = Regex.Matches(inputString, _date);
                    if (matches.Count > 0)
                        return matches[1].Value;
                    return null;
                },
                _ => throw new ArgumentException("Invalid incoming data of ParserTypes.")
            };

        private static string GetSubStringFromString(string inputString, string startWith)
        {
            string[] subStrings = inputString.Split(' ');
            foreach (var item in subStrings)
                if(item.StartsWith(startWith))
                    return item;
            return null;
        }

        private static string GetSTRINGFromSubString(string inputSubString, ParserTypes typeOfParse)
            => ParserFactory.GetMatch(inputSubString, ParserFactory.GetPattern(typeOfParse));

        internal static int SearchSubstringINT(string inputSubString, string startWith, ParserTypes typeOfParse)
            => Int32.Parse(GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith), typeOfParse));

        internal static IPAddress SearchSubStringIP(string inputSubString, string startWith, ParserTypes typeOfParse)
            => IPAddress.Parse(GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith), typeOfParse));

        internal static string[] ReplacePatterns(string filters)
        {
            StringBuilder patterns = new StringBuilder(filters);

            patterns.Replace("STRING", _string);
            patterns.Replace("INT", _int);
            patterns.Replace("DATE", _date);
            patterns.Replace("TIME", _time);
            patterns.Replace("GUID", _guid);
            patterns.Replace("MAC", _mac);
            patterns.Replace("IP", _ip);

            patterns.Replace(";", string.Empty);

            return patterns.ToString().Split(' ');
        }

        internal static string[] GetKeysFromConfigurationFilters(string value)
        {
            StringBuilder keys = new StringBuilder();
            string[] substrings = value.Split(' ');
            foreach (var item in substrings)
            {
                keys.Append(item.Split('=')[0]);
                keys.Append(' ');
            }
            return keys.ToString().Split(' ');
        }

        internal static string[] GetPatternsFromConfigurationFilters(string value)
        {
            StringBuilder patterns = new StringBuilder();
            string[] substrings = value.Split(' ');
            foreach (var item in substrings)
            {
                patterns.Append(item.Split('=')[1]);
                patterns.Append(' ');
            }
            patterns.Replace(";", string.Empty);
            patterns.Replace("\"", string.Empty);
            return patterns.ToString().Split(' ');
        }

        internal static JsonObject GetParsedDictionary(string message, IConfigurationFilters configurationFilters, string[] patterns)
        {
            JsonObject output = new JsonObject();

            for (int i = 0; i < patterns.Length; i++)
            {
                string subStringFromMessage = GetMatch(message, patterns[i]);
                ParserTypes pattern = Enum.Parse<ParserTypes>(configurationFilters.FilterPatterns[i]);
                object value = GetObjectByPattern(subStringFromMessage, pattern);
                System.Console.WriteLine("Key: {0}, Value: {1}, SubString: {2}, Pattern: {3}", configurationFilters.FilterKeys[i], value.ToString(),subStringFromMessage, pattern);
            }

            return output;
        }
    }
}