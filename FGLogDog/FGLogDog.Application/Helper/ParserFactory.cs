using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Net;

namespace FGLogDog.Application.Helper
{
    public static class ParserFactory
    {
        private static string _int = @"{0}(\d+)";
        private static string _logid = @"{0}""(\d+)""";
        private static string _time = @"{0}([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        private static string _date = @"{0}([12]\d{{3}}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        private static string _string = @"{0}""([^""\\]*(?:\\.[^""\\]*)*)""";
        private static string _guid = @"{0}""([({{]?[a-fA-F0-9]{{8}}[-]?([a-fA-F0-9]{{4}}[-]?){{3}}[a-fA-F0-9]{{12}}[}})]?)""";
        private static string _mac = @"{0}""(?:[0-9A-Fa-f]{{2}}[:-]){{5}}(?:[0-9A-Fa-f]{{2}})""";
        private static string _ip = @"{0}(\b\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\b)";
        private static string _syslog = @"^([A-Za-z]{3})\s(0[1-9]|[12]\d|3[01])\s([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";

        public static string GetPattern(ParserTypes type, string prefix = null)
            => type switch
            {
                ParserTypes.INT => string.Format(_int, prefix),
                ParserTypes.TIME => string.Format(_time, prefix),
                ParserTypes.DATE => string.Format(_date, prefix),
                ParserTypes.STRING => string.Format(_string, prefix),
                ParserTypes.GUID => string.Format(_guid, prefix),
                ParserTypes.MAC => string.Format(_mac, prefix),
                ParserTypes.IP => string.Format(_ip, prefix),
                ParserTypes.SYSLOG => _syslog,
                ParserTypes.LOGID => string.Format(_logid, prefix),
                _ => throw new ArgumentException("Invalid incoming data of ParserTypes.")
            };
        
        public static string GetMatch(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
                return matches.First().Value;
            return null;
        }

        public static int GetSubstringINT(string inputString, string subStr, ParserTypes typeOfParse)
        {
            string[] subs = inputString.Split(' ');
            foreach (var item in subs)
            {
                if (ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse, subStr)) is not null)
                    return Int32.Parse(ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse)));
            }
            return 0;
        }

        public static IPAddress GetSubstringIP(string inputString, string subStr, ParserTypes typeOfParse)
        {
            string[] subs = inputString.Split(' ');
            foreach (var item in subs)
            {
                if (ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse, subStr)) is not null)
                    return IPAddress.Parse(ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse)));
            }
            return null;
        }

        public static string GetSubstringSTRING(string inputString, string subStr, ParserTypes typeOfParse)
        {
            string[] subs = inputString.Split(' ');
            foreach (var item in subs)
            {
                if (ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse, subStr)) is not null)
                    return ParserFactory.GetMatch(item, ParserFactory.GetPattern(typeOfParse));
            }
            return null;
        }
    }
}