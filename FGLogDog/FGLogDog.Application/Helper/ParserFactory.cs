using System;
namespace FGLogDog.Application.Helper
{
    public static class ParserFactory
    {
        private static string _int = @"\s{0}=(\d+)";
        private static string _logid = @"\s{0}=""(\d+)""";
        private static string _time = @"\s{0}=([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))";
        private static string _date = @"\s{0}=([12]\d{{3}}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))"; 
        private static string _string = @"\s{0}=""([^""\\]*(?:\\.[^""\\]*)*)""";
        private static string _guid = @"\s{0}=""([({{]?[a-fA-F0-9]{{8}}[-]?([a-fA-F0-9]{{4}}[-]?){{3}}[a-fA-F0-9]{{12}}[}})]?)""";
        private static string _mac = @"\s{0}=""(?:[0-9A-Fa-f]{{2}}[:-]){{5}}(?:[0-9A-Fa-f]{{2}})""";
        private static string _ip = @"\s{0}=(\b\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\.\d{{1,3}}\b)";
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
    }
}