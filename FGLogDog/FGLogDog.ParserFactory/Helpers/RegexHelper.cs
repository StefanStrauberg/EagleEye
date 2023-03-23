using System;
using System.Text;
using FGLogDog.Application.Models;

namespace FGLogDog.ParserFactory.Helpers
{
    internal static class RegexHelper
    {
        internal const string _int = @"(\d+)";
        internal const string _time = @"([0-2]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        internal const string _date = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        internal const string _string = @"([^""\\]*(?:\\.[^""\\]*)*)";
        internal const string _guid = @"([({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?)";
        internal const string _mac = @"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})";
        internal const string _ip = @"(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)";

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
    }
}