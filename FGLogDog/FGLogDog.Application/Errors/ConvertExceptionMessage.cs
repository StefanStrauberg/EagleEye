using System;

namespace FGLogDog.Application.Errors
{
    /// <summary>
    /// General class for parsing exception
    /// </summary>
    public static class ConvertExceptionMessage
    {
        /// <summary>
        /// Convert full exception to short exception for logging. Present internal exception messages
        /// </summary>
        /// <param name="e">Incoming exception</param>
        /// <param name="msgs">Prefix for output short exception, default empty string</param>
        /// <returns>InnerException: An invalid argument was supplied.</returns>
        public static string GetExceptionMessages(Exception e, string msgs = "")
        {
            if (e == null) return string.Empty;
            if (msgs == "") msgs = e.Message;
            if (e.InnerException != null)
                msgs += "\r\nInnerException: " + GetExceptionMessages(e.InnerException);
            return msgs;
        }
    }
}