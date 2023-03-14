using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace FGLogDog.Application.Helper
{
    internal static class ParserFactory
    {
        const string _int = @"(\d+)";
        const string _time = @"([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        const string _date = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        const string _string = @"([^""\\]*(?:\\.[^""\\]*)*)";
        const string _guid = @"([({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?)";
        const string _mac = @"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})";
        const string _ip = @"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

        static string GetPattern(ParserTypes type)
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
        
        static string GetMatch(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
                return matches.First().Value;
            return null;
        }

        static string GetSubStringFromString(string inputString, string startWith)
        {
            string[] subStrings = inputString.Split(' ');
            foreach (var item in subStrings)
                if(item.StartsWith(startWith))
                    return item;
            return null;
        }

        static string GetSTRINGFromSubString(string inputSubString, ParserTypes typeOfParse)
            => ParserFactory.GetMatch(inputSubString, ParserFactory.GetPattern(typeOfParse));

        internal static int GetINT(string inputSubString, string startWith)
            => Int32.Parse(GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith), ParserTypes.INT));

        internal static IPAddress GetIP(string inputSubString, string startWith)
            => IPAddress.Parse(GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith), ParserTypes.IP));

        internal static string GetSTRING(string inputSubString, string startWith)
            => GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith).Split('=')[1], ParserTypes.STRING).Replace(";", string.Empty);

        internal static string[] ReplaceReadablePatterns(string filters)
        {
            string[] substrings = filters.Split(' ');
            string[] output = new string[substrings.Length];
            int startIndex;
            int length;
            for (int i = 0; i < substrings.Length; i++)
            {
                StringBuilder sb = new StringBuilder(substrings[i].Split("=>")[1]);
                sb.Replace("{", string.Empty);
                sb.Replace("}", string.Empty);
                startIndex = sb.ToString().IndexOf(':');
                length = sb.Length - startIndex;
                sb.Remove(startIndex, length);

                sb.Replace("STRING", _string);
                sb.Replace("INT", _int);
                sb.Replace("DATE", _date);
                sb.Replace("TIME", _time);
                sb.Replace("GUID", _guid);
                sb.Replace("MAC", _mac);
                sb.Replace("IP", _ip);
                
                System.Console.WriteLine(sb.ToString());
                output[i] = sb.ToString();
            }

            return output;
        }

        internal static string[] GetKeysFromConfigurationFilters(string value)
        {
            string[] substrings = value.Split(' ');
            string[] output = new string[substrings.Length];
            for (int i = 0; i < substrings.Length; i++)
                output[i] = substrings[i].Split("=>")[0];
            return output;
        }

        internal static string[] GetPatternsFromConfigurationFilters(string value)
        {
            string[] substrings = value.Split(' ');
            string[] output = new string[substrings.Length];
            for (int i = 0; i < substrings.Length; i++)
            {
                StringBuilder sb = new StringBuilder(substrings[i].Split("=>")[1]);
                sb.Replace("{", string.Empty);
                sb.Replace("}", string.Empty);
                var endIndex = sb.ToString().IndexOf(':');
                sb.Remove(0, ++endIndex);
                output[i] = sb.ToString();
            }
            return output;
        }

        internal static BsonDocument GetJsonFromMessage(string message, IConfigurationFilters filters)
        {
            BsonDocument bson = new BsonDocument();

            for (int i = 0; i < filters.Patterns.Length; i++)
            {
                string inputString = GetMatch(message, filters.Patterns[i]);
                ParserTypes pattern = Enum.Parse<ParserTypes>(filters.FilterPatterns[i]);

                MatchCollection matches;

                switch (pattern)
                {
                    case ParserTypes.INT:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _int);
                        if (matches.Count > 0)
                            if (Int32.TryParse(matches.First().Value, out var data))
                                bson.Add(new BsonElement(filters.FilterKeys[i], data));
                        break;
                    case ParserTypes.TIME:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _time);
                        if (matches.Count > 0)
                            if (TimeOnly.TryParse(matches.First().Value, out var data))
                                bson.Add(new BsonElement(filters.FilterKeys[i], data.ToString()));
                        break;
                    case ParserTypes.DATE:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _date);
                        if (matches.Count > 0)
                            if (DateOnly.TryParse(matches.First().Value, out var data))
                                bson.Add(new BsonElement(filters.FilterKeys[i], data.ToString()));
                        break;
                    case ParserTypes.STRING:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _string);
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(filters.FilterKeys[i], matches[2].Value));
                        break;
                    case ParserTypes.GUID:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _guid);
                        if (matches.Count > 0)
                            if (Guid.TryParse(matches.First().Value, out var data))
                                bson.Add(new BsonElement(filters.FilterKeys[i], data));
                        break;
                    case ParserTypes.MAC:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _mac);
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(filters.FilterKeys[i], matches[0].Value));
                        break;
                    case ParserTypes.IP:
                        if (string.IsNullOrWhiteSpace(inputString))
                            break;
                        matches = Regex.Matches(inputString, _ip);
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(filters.FilterKeys[i], matches[0].Value));
                        break;
                    default:
                        throw new ArgumentException("Invalid incoming data of ParserTypes.");
                }
            }
            return bson;
        }
    }
}