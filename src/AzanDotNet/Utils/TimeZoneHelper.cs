using System.Globalization;

namespace AzanDotNet;

/// <summary>
/// Helper class for handling timezone conversions and daylight saving time
/// </summary>
public static class TimeZoneHelper
{
    /// <summary>
    /// Converts UTC prayer times to local timezone with automatic DST handling
    /// </summary>
    /// <param name="utcTime">UTC time</param>
    /// <param name="timeZoneId">IANA timezone ID (e.g., "America/New_York", "Europe/London")</param>
    /// <returns>Local time with DST applied</returns>
    public static DateTime ConvertToTimeZone(DateTime utcTime, string timeZoneId)
    {
        if (utcTime == DateTime.MinValue)
            return DateTime.MinValue;

        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ConvertIanaToWindows(timeZoneId));
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        }
        catch
        {
            // Fallback: try direct IANA ID (works on Linux/macOS)
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }

    /// <summary>
    /// Converts UTC prayer times to local timezone using TimeZoneInfo
    /// </summary>
    /// <param name="utcTime">UTC time</param>
    /// <param name="timeZone">TimeZoneInfo object</param>
    /// <returns>Local time with DST applied</returns>
    public static DateTime ConvertToTimeZone(DateTime utcTime, TimeZoneInfo timeZone)
    {
        if (utcTime == DateTime.MinValue)
            return DateTime.MinValue;

        try
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Gets timezone offset including DST for a specific date
    /// </summary>
    /// <param name="date">Date to check</param>
    /// <param name="timeZoneId">IANA timezone ID</param>
    /// <returns>Offset from UTC including DST</returns>
    public static TimeSpan GetTimezoneOffset(DateTime date, string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ConvertIanaToWindows(timeZoneId));
            return timeZone.GetUtcOffset(date);
        }
        catch
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return timeZone.GetUtcOffset(date);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }
    }

    /// <summary>
    /// Checks if daylight saving time is in effect for a specific date and timezone
    /// </summary>
    /// <param name="date">Date to check</param>
    /// <param name="timeZoneId">IANA timezone ID</param>
    /// <returns>True if DST is in effect</returns>
    public static bool IsDaylightSavingTime(DateTime date, string timeZoneId)
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(ConvertIanaToWindows(timeZoneId));
            return timeZone.IsDaylightSavingTime(date);
        }
        catch
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return timeZone.IsDaylightSavingTime(date);
            }
            catch
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Converts IANA timezone ID to Windows timezone ID
    /// </summary>
    /// <param name="ianaId">IANA timezone ID</param>
    /// <returns>Windows timezone ID</returns>
    public static string ConvertIanaToWindows(string ianaId)
    {
        // Common IANA to Windows timezone mappings
        var mappings = new Dictionary<string, string>
        {
            // Americas
            { "America/New_York", "Eastern Standard Time" },
            { "America/Chicago", "Central Standard Time" },
            { "America/Denver", "Mountain Standard Time" },
            { "America/Los_Angeles", "Pacific Standard Time" },
            { "America/Toronto", "Eastern Standard Time" },
            { "America/Vancouver", "Pacific Standard Time" },
            { "America/Sao_Paulo", "E. South America Standard Time" },
            
            // Europe
            { "Europe/London", "GMT Standard Time" },
            { "Europe/Paris", "W. Europe Standard Time" },
            { "Europe/Berlin", "W. Europe Standard Time" },
            { "Europe/Rome", "W. Europe Standard Time" },
            { "Europe/Madrid", "W. Europe Standard Time" },
            { "Europe/Amsterdam", "W. Europe Standard Time" },
            { "Europe/Brussels", "W. Europe Standard Time" },
            { "Europe/Vienna", "W. Europe Standard Time" },
            { "Europe/Prague", "Central Europe Standard Time" },
            { "Europe/Warsaw", "Central European Standard Time" },
            { "Europe/Moscow", "Russian Standard Time" },
            { "Europe/Istanbul", "Turkey Standard Time" },
            
            // Asia
            { "Asia/Dubai", "Arabian Standard Time" },
            { "Asia/Riyadh", "Arab Standard Time" },
            { "Asia/Kuwait", "Arab Standard Time" },
            { "Asia/Qatar", "Arab Standard Time" },
            { "Asia/Bahrain", "Arab Standard Time" },
            { "Asia/Tehran", "Iran Standard Time" },
            { "Asia/Karachi", "Pakistan Standard Time" },
            { "Asia/Kolkata", "India Standard Time" },
            { "Asia/Dhaka", "Bangladesh Standard Time" },
            { "Asia/Jakarta", "SE Asia Standard Time" },
            { "Asia/Singapore", "Singapore Standard Time" },
            { "Asia/Kuala_Lumpur", "Singapore Standard Time" },
            { "Asia/Bangkok", "SE Asia Standard Time" },
            { "Asia/Manila", "Singapore Standard Time" },
            { "Asia/Tokyo", "Tokyo Standard Time" },
            { "Asia/Seoul", "Korea Standard Time" },
            { "Asia/Shanghai", "China Standard Time" },
            { "Asia/Hong_Kong", "China Standard Time" },
            
            // Africa
            { "Africa/Cairo", "Egypt Standard Time" },
            { "Africa/Casablanca", "Morocco Standard Time" },
            { "Africa/Lagos", "W. Central Africa Standard Time" },
            { "Africa/Johannesburg", "South Africa Standard Time" },
            
            // Australia
            { "Australia/Sydney", "AUS Eastern Standard Time" },
            { "Australia/Melbourne", "AUS Eastern Standard Time" },
            { "Australia/Perth", "W. Australia Standard Time" },
            { "Australia/Adelaide", "Cen. Australia Standard Time" },
            
            // Pacific
            { "Pacific/Auckland", "New Zealand Standard Time" },
            { "Pacific/Fiji", "Fiji Standard Time" }
        };

        return mappings.TryGetValue(ianaId, out var windowsId) ? windowsId : ianaId;
    }

    /// <summary>
    /// Gets all available timezone IDs on the current system
    /// </summary>
    /// <returns>List of timezone IDs</returns>
    public static IEnumerable<string> GetAvailableTimeZones()
    {
        return TimeZoneInfo.GetSystemTimeZones().Select(tz => tz.Id);
    }

    /// <summary>
    /// Formats a DateTime with timezone information
    /// </summary>
    /// <param name="dateTime">DateTime to format</param>
    /// <param name="timeZoneId">Timezone ID</param>
    /// <param name="format">Format string (default: "HH:mm")</param>
    /// <returns>Formatted time string with timezone info</returns>
    public static string FormatWithTimeZone(DateTime dateTime, string timeZoneId, string format = "HH:mm")
    {
        if (dateTime == DateTime.MinValue)
            return "N/A";

        try
        {
            var localTime = ConvertToTimeZone(dateTime, timeZoneId);
            var isDst = IsDaylightSavingTime(localTime, timeZoneId);
            var offset = GetTimezoneOffset(localTime, timeZoneId);
            
            var timeStr = localTime.ToString(format);
            var offsetStr = $"UTC{offset.Hours:+0;-0}:{Math.Abs(offset.Minutes):D2}";
            var dstStr = isDst ? " (DST)" : "";
            
            return $"{timeStr} {offsetStr}{dstStr}";
        }
        catch
        {
            return "N/A";
        }
    }
}
