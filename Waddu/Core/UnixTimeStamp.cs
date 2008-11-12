using System;

namespace Waddu.Core
{
    public class UnixTimeStamp
    {
        private UnixTimeStamp() { }

        public static long GetUnixTimeStamp(DateTime dtToConvert)
        {
            DateTime dtBase = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = dtToConvert.ToUniversalTime() - dtBase;
            return (long)span.TotalSeconds;
        }

        public static DateTime GetDateTime(double unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        } 
    }
}
