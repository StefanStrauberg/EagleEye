using FGLogDog.FGLogDog.Application.Helper;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FGLogDog.Application.Helper
{
    internal static partial class ParserFactory
    {
        private static IConfigurationFilters _filters;
        
        public static void InitFilter(IConfigurationFilters filters)
            => _filters = filters;

        const string _int = @"(\d+)";
        const string _time = @"([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        const string _date = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        const string _string = @"([^""\\]*(?:\\.[^""\\]*)*)";
        const string _guid = @"([({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?)";
        const string _mac = @"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})";
        const string _ip = @"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";
        
        static string GetMatch(string input, string pattern)
        {
            var matches = Regex.Matches(input, pattern);
            if (matches.Count > 0)
                return matches.First().Value;
            return null;
        }

        internal static string[] ReplaceReadablePatterns(string filters)
        {
            string[] substrings = filters.Split(' ');
            string[] output = new string[substrings.Length];
            int startIndex;
            int length;
            for (int i = 0; i < substrings.Length; i++)
            {
                StringBuilder sb = new(substrings[i].Split("=>")[1]);
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

        internal static ParserTypes[] GetPatternsFromConfigurationFilters(string value)
        {
            string[] substrings = value.Split(' ');
            ParserTypes[] output = new ParserTypes[substrings.Length];
            for (int i = 0; i < substrings.Length; i++)
            {
                StringBuilder sb = new(substrings[i].Split("=>")[1]);
                sb.Replace("{", string.Empty);
                sb.Replace("}", string.Empty);
                var endIndex = sb.ToString().IndexOf(':');
                sb.Remove(0, ++endIndex);
                output[i] = Enum.Parse<ParserTypes>(sb.ToString());
            }
            return output;
        }

        internal static byte[] GetMessage(byte[] bytes)
        {
            string message = Encoding.UTF8.GetString(bytes);

            StringBuilder inputString = new();

            BsonDocument bson = new();

            MatchCollection matches;

            for (int i = 0; i < _filters.SearchableSubStrings.Length; i++)
            {
                inputString.Clear();

                inputString.Append(GetMatch(message, _filters.SearchableSubStrings[i]));

                if (inputString.ToString() is null)
                    continue;

                switch (_filters.FilterPatterns[i])
                {
                    case ParserTypes.INT:
                        matches = MyRegex().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], Int32.Parse(matches[0].Value)));
                        break;
                    case ParserTypes.TIME:
                        matches = MyRegex1().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], matches[0].Value));
                        break;
                    case ParserTypes.DATE:
                        matches = MyRegex2().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], matches[0].Value));
                        break;
                    case ParserTypes.STRING:
                        matches = MyRegex3().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], matches[2].Value));
                        break;
                    case ParserTypes.GUID:
                        matches = MyRegex4().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i],
                                                     new BsonBinaryData(Guid.Parse(matches[0].Value),
                                                                        GuidRepresentation.Standard)));
                        break;
                    case ParserTypes.MAC:
                        matches = MyRegex5().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], matches[0].Value));
                        break;
                    case ParserTypes.IP:
                        matches = MyRegex6().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            bson.Add(new BsonElement(_filters.FilterKeys[i], matches[0].Value));
                        break;
                    case ParserTypes.DATETIME:
                        StringBuilder temp = new();
                        matches = MyRegex7().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            temp.Append(matches[0].Value);
                        else
                            break;
                        temp.Append(' ');
                        matches = MyRegex8().Matches(inputString.ToString());
                        if (matches.Count > 0)
                            temp.Append(matches[0].Value);
                        else
                            break;
                            bson.Add(new BsonElement(_filters.FilterKeys[i], DateTime.Parse(temp.ToString())));
                        break;
                    default:
                        throw new ArgumentException("Invalid incoming data of ParserTypes.");
                }
            }
            if (bson.ElementCount is 0)
                return null;    
            return Encoding.UTF8.GetBytes(bson.ToString());
        }

        [GeneratedRegex("(\\d+)")]
        private static partial Regex MyRegex();
        [GeneratedRegex("([0-1]?\\d|2[0-3])(?::([0-5]?\\d))?(?::([0-5]?\\d))")]
        private static partial Regex MyRegex1();
        [GeneratedRegex("([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01]))")]
        private static partial Regex MyRegex2();
        [GeneratedRegex("([^\"\\\\]*(?:\\\\.[^\"\\\\]*)*)")]
        private static partial Regex MyRegex3();
        [GeneratedRegex("([({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?)")]
        private static partial Regex MyRegex4();
        [GeneratedRegex("(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})")]
        private static partial Regex MyRegex5();
        [GeneratedRegex("(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")]
        private static partial Regex MyRegex6();
        [GeneratedRegex("([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01]))")]
        private static partial Regex MyRegex7();
        [GeneratedRegex("([0-1]?\\d|2[0-3])(?::([0-5]?\\d))?(?::([0-5]?\\d))")]
        private static partial Regex MyRegex8();
    }
}