using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FGLogDog.Application.Helper;

namespace FGLogDog.Application.DataStore
{
    public class FakeDataStore
    {

        public Task<string> GetLog()
        {
            //var line = await Task.FromResult(_inputLogs[_rnd.Next(0,_inputLogs.Count)]);
            var input = "Feb 15 15:13:56 fg_600 date=2023-02-15 time=15:13:56 devname=\"dc1-utm1-1\" devid=\"FG6H0ETB20901717\" eventtime=1676463236038465512 tz=\"+0300\" logid=\"0000000013\" type=\"traffic\" subtype=\"forward\" level=\"notice\" vd=\"CORP_NETW\" srcip=10.140.13.101 srcport=40120 srcintf=\"e1012_LVStoFG\" srcintfrole=\"lan\" dstip=142.250.186.170 dstport=443 dstintf=\"FG702_CORP_NETW\" dstintfrole=\"lan\" srccountry=\"Reserved\" dstcountry=\"Germany\" sessionid=1624386702 proto=6 action=\"client-rst\" policyid=6 policytype=\"policy\" poluuid=\"2d31772c-0eed-51ec-d3c8-f0ef4d353f8d\" policyname=\"Vitebsk_to_Internet\" service=\"HTTPS\" trandisp=\"snat\" transip=93.84.112.7 transport=40120 appid=42533 app=\"Google.Services\" appcat=\"General.Interest\" apprisk=\"elevated\" applist=\"deny_soc&remote&proxy\" duration=6 sentbyte=2683 rcvdbyte=2282 sentpkt=25 rcvdpkt=16 utmaction=\"allow\" countapp=1 mastersrcmac=\"02:04:96:52:ee:89\" srcmac=\"02:04:96:52:ee:89\" srcserver=0 masterdstmac=\"40:a6:77:42:93:f0\" dstmac=\"40:a6:77:42:93:f0\" dstserver=0";

            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.SYSLOG));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.DATE, "date"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.TIME, "time"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "devname"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "devid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.LOGID, "logid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "type"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "subtype"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "level"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "vd"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.IP, "srcip"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "srcport"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "srcintf"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "srcintfrole"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.IP, "dstip"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "dstport"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "dstintf"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "dstintfrole"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "srccountry"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "dstcountry"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "sessionid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "proto"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "action"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "policyid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "policytype"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.GUID, "poluuid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "policyname"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "service"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "trandisp"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "transip"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "transport"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "appid"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "app"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "appcat"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "apprisk"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "applist"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "duration"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "sentbyte"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "rcvdbyte"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "sentpkt"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "rcvdpkt"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.STRING, "utmaction"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "countapp"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.MAC, "mastersrcmac"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.MAC, "srcmac"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "srcserver"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.MAC, "masterdstmac"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.MAC, "dstmac"));
            PrintMatches(input, ParserFactory.GetPattern(ParserTypes.INT, "dstserver"));

            return Task.FromResult(input);
        }
        private void PrintMatches(string input, string pattern)
        {
            foreach (Match m in Regex.Matches(input, pattern))
            {
                Console.WriteLine($"{m.Value}");
            }
            System.Console.WriteLine(new string('*',50));
        }
    }
}