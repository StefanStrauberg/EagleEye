using System.Text;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;

namespace FGLogDog.Application.Helper
{
    public static class ParserFactory
    {
        private const string _int = @"(\d+)";
        private const string _logid = @"(\d+)";
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
                ParserTypes.LOGID => _logid,
                _ => throw new ArgumentException("Invalid incoming data of ParserTypes.")
            };
        
        private static string GetMatch(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
                return matches.First().Value;
            return null;
        }

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

        internal static string SearchSubstringSTRING(string inputSubString, string startWith, ParserTypes typeOfParse)
            =>  GetSTRINGFromSubString(GetSubStringFromString(inputSubString, startWith), typeOfParse);

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

        internal static IDictionary<string, object> GetParsedDictionary(string message, string[] filters, string[] patterns)
        {
            
            Dictionary<string, object> output = new Dictionary<string, object>();

            for (int i = 0; i < patterns.Length; i++)
            {
                string test = GetMatch(message, patterns[i]);
                System.Console.WriteLine(test);
            }

            return output;
        }
    }
}