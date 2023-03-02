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
            Regex regex = new Regex(@"ObjectId\(([^\)]*)\)");
            MatchCollection matches = regex.Matches(inputJsonString);
            if (matches.Count > 0)
                sb.Replace("ObjectId(", string.Empty).Replace(")", String.Empty);
            return sb.ToString();
        }
    }
}