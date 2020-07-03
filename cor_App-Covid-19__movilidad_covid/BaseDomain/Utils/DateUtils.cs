using System;

namespace Domain.Utils
{
    public static class DateUtils
    {
        public static DateTime UnixToDateTime(this long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).LocalDateTime;
        }

        public static long DateTimeToUnix(this DateTime date)
        {
            return new DateTimeOffset(date).ToUnixTimeMilliseconds();
        }
    }
}
