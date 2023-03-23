using System;
using System.Text;
using System.Text.RegularExpressions;

namespace EagleEye.Application.Helpers
{
    public static class JsonHelper
    {
        public static string Correction(string inputJsonString)
        {
            if (string.IsNullOrEmpty(inputJsonString))
                return string.Empty;
            
            StringBuilder sb = new StringBuilder(inputJsonString);
            
            Regex regexObjectId = new Regex(@"ObjectId\(([^\)]*)\)");
            Regex regexISODate = new Regex(@"ISODate\(([^\)]*)\)");
            Regex regexCSUUID = new Regex(@"CSUUID\(([^\)]*)\)");
            
            MatchCollection matchesObjectId = regexObjectId.Matches(inputJsonString);
            if (matchesObjectId.Count > 0)
                sb.Replace("ObjectId(", string.Empty).Replace(")", String.Empty);

            MatchCollection matchesISODate = regexISODate.Matches(inputJsonString);
            if (matchesISODate.Count > 0)
                sb.Replace("ISODate(", string.Empty).Replace(")", String.Empty);

            MatchCollection matchesCSUUID = regexCSUUID.Matches(inputJsonString);
            if (matchesCSUUID.Count > 0)
                sb.Replace("CSUUID(", string.Empty).Replace(")", String.Empty);

            return sb.ToString();
        }
    }
}