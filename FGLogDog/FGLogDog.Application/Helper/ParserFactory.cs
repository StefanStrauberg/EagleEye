using FGLogDog.Application.Models;
using FGLogDog.FGLogDog.Application.Helper;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
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

        //static Func<TResult> GetObjectByPattern<TResult>(string inputString, ParserTypes pattern)
        //    => pattern switch
        //    {
        //        ParserTypes.DATE => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _date);
        //            if (matches.Count > 0)
        //                if (DateOnly.TryParse(matches.First().Value, out var data))
        //                    return data;
        //            return DateOnly.MinValue;
        //        }
        //        ,
        //        ParserTypes.TIME => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _time);
        //            if (matches.Count > 0)
        //                if (TimeOnly.TryParse(matches.First().Value, out var data))
        //                    return data;
        //            return TimeOnly.MinValue;
        //        }
        //        ,
        //        ParserTypes.INT => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _int);
        //            if (matches.Count > 0)
        //                if (Int32.TryParse(matches.First().Value, out var data))
        //                    return data;
        //            return 0;
        //        }
        //        ,
        //        ParserTypes.GUID => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _guid);
        //            if (matches.Count > 0)
        //                if (Guid.TryParse(matches.First().Value, out var data))
        //                    return data;
        //            return Guid.Empty;
        //        }
        //        ,
        //        ParserTypes.MAC => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _mac);
        //            if (matches.Count > 0)
        //                return matches.First().Value;
        //            return null;
        //        }
        //        ,
        //        ParserTypes.IP => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _ip);
        //            if (matches.Count > 0)
        //                if (IPAddress.TryParse(matches.First().Value, out var data))
        //                    return data.ToString();
        //            return null;
        //        }
        //        ,
        //        ParserTypes.STRING => () =>
        //        {
        //            if (string.IsNullOrWhiteSpace(inputString)) return null;
        //            var matches = Regex.Matches(inputString, _string);
        //            if (matches.Count > 0)
        //                return matches[2].Value;
        //            return null;
        //        }
        //        ,
        //        _ => throw new ArgumentException("Invalid incoming data of ParserTypes.")
        //    };

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
            StringBuilder patterns = new StringBuilder(filters);

            patterns.Replace("STRING", _string);
            patterns.Replace("INT", _int);
            patterns.Replace("DATE", _date);
            patterns.Replace("TIME", _time);
            patterns.Replace("GUID", _guid);
            patterns.Replace("MAC", _mac);
            patterns.Replace("IP", _ip);

            patterns.Replace(";", string.Empty);

            string[] output = patterns.ToString().Split(' ');
            return output;
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
            string[] output = keys.ToString().Split(' ');
            return output;
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
            string[] output = patterns.ToString().Split(' ');
            return output;
        }

        internal static BsonDocument GetJsonFromMessage(string message, IConfigurationFilters filters)
        {
            BsonDocument bson = new BsonDocument();

            for (int i = 0; i < filters.Patterns.Length; i++)
            {
                string inputString = GetMatch(message, filters.Patterns[i]);
                ParserTypes pattern = Enum.Parse<ParserTypes>(filters.FilterPatterns[i]);
                //var value = GetObjectByPattern(inputString, pattern);

                switch (pattern)
                {
                    case ParserTypes.INT:
                        break;
                    case ParserTypes.TIME:
                        break;
                    case ParserTypes.DATE:
                        break;
                    case ParserTypes.STRING:
                        break;
                    case ParserTypes.GUID:
                        break;
                    case ParserTypes.MAC:
                        break;
                    case ParserTypes.IP:
                        break;
                    case ParserTypes.SYSLOG:
                        break;
                    case ParserTypes.LOGID:
                        break;
                    default:
                        throw new ArgumentException("Invalid incoming data of ParserTypes.");
                }

                //if (value is null)
                //    continue;

                //output.Add(filters.FilterKeys[i], JsonValue.Create(value));
            }

            //if (output.Count > 0)
            //    return output;

            return null;
        }
    }
}