using System;

namespace Waddu.Core
{
    public static class UnixTimeStamp
    {
        public static long GetUnixTimeStamp(DateTime dtToConvert)
        {
            var dtBase = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var span = dtToConvert.ToUniversalTime() - dtBase;
            return (long)span.TotalSeconds;
        }

        public static DateTime GetDateTime(double unixTime)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTime).ToLocalTime();
        } 
    }
}
