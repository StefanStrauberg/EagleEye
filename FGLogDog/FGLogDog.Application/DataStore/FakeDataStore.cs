using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FGLogDog.Application.DataStore
{
    public class FakeDataStore
    {
        private static List<string> _inputLogs;
        private readonly Random _rnd;

        public FakeDataStore()
        {
            _rnd = new Random();
            _inputLogs = new List<string>();
            var dirDatabaseScripts = Path.Combine(Directory.GetCurrentDirectory(), "input.log");
            var data = File.ReadAllLines(dirDatabaseScripts);
            foreach (var item in data)
                _inputLogs.Add(item);
        }

        public async Task<string> GetLog()
        {
            //var line = await Task.FromResult(_inputLogs[_rnd.Next(0,_inputLogs.Count)]);
            var line = "Feb 15 15:13:56 fg_600 date=2023-02-15 time=15:13:56 devname=\"dc1-utm1-1\" devid=\"FG6H0ETB20901717\" eventtime=1676463236038465512 tz=\"+0300\" logid=\"0000000013\" type=\"traffic\" subtype=\"forward\" level=\"notice\" vd=\"CORP_NETW\" srcip=10.140.13.101 srcport=40120 srcintf=\"e1012_LVStoFG\" srcintfrole=\"lan\" dstip=142.250.186.170 dstport=443 dstintf=\"FG702_CORP_NETW\" dstintfrole=\"lan\" srccountry=\"Reserved\" dstcountry=\"Germany\" sessionid=1624386702 proto=6 action=\"client-rst\" policyid=6 policytype=\"policy\" poluuid=\"2d31772c-0eed-51ec-d3c8-f0ef4d353f8d\" policyname=\"Vitebsk_to_Internet\" service=\"HTTPS\" trandisp=\"snat\" transip=93.84.112.7 transport=40120 appid=42533 app=\"Google.Services\" appcat=\"General.Interest\" apprisk=\"elevated\" applist=\"deny_soc&remote&proxy\" duration=6 sentbyte=2683 rcvdbyte=2282 sentpkt=25 rcvdpkt=16 utmaction=\"allow\" countapp=1 mastersrcmac=\"02:04:96:52:ee:89\" srcmac=\"02:04:96:52:ee:89\" srcserver=0 masterdstmac=\"40:a6:77:42:93:f0\" dstmac=\"40:a6:77:42:93:f0\" dstserver=0";
            Regex regex = new Regex("^Feb (\\d+) (([0-1]?\\d|2[0-3])(?::([0-5]?\\d))?(?::([0-5]?\\d))) fg_600 date=([12]\\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\\d|3[01])) time=([0-1]?\\d|2[0-3])(?::([0-5]?\\d))?(?::([0-5]?\\d)) devname=\"dc1-utm1-1\" devid=\"FG6H0ETB20901717\" eventtime=(\\d+) tz=\"+0300\" logid=\"0000000013\" type=\"traffic\" subtype=\"forward\" level=\"notice\" vd=\"CORP_NETW\" srcip=10.140.13.101 srcport=40120 srcintf=\"e1012_LVStoFG\" srcintfrole=\"lan\" dstip=142.250.186.170 dstport=443 dstintf=\"FG702_CORP_NETW\" dstintfrole=\"lan\" srccountry=\"Reserved\" dstcountry=\"Germany\" sessionid=1624386702 proto=6 action=\"client-rst\" policyid=6 policytype=\"policy\" poluuid=\"2d31772c-0eed-51ec-d3c8-f0ef4d353f8d\" policyname=\"Vitebsk_to_Internet\" service=\"HTTPS\" trandisp=\"snat\" transip=93.84.112.7 transport=40120 appid=42533 app=\"Google.Services\" appcat=\"General.Interest\" apprisk=\"elevated\" applist=\"deny_soc&remote&proxy\" duration=6 sentbyte=2683 rcvdbyte=2282 sentpkt=25 rcvdpkt=16 utmaction=\"allow\" countapp=1 mastersrcmac=\"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})\" srcmac=\"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})\" srcserver=0 masterdstmac=\"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})\" dstmac=\"(?:[0-9A-Fa-f]{2}[:-]){5}(?:[0-9A-Fa-f]{2})\" dstserver=0$");
            Match match = regex.Match(line);

            return line;
        }
    }
}