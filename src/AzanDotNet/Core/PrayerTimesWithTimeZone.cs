namespace AzanDotNet;

/// <summary>
/// Prayer times with automatic timezone conversion and DST handling
/// </summary>
public class PrayerTimesWithTimeZone
{
    private readonly PrayerTimes _utcPrayerTimes;
    private readonly string _timeZoneId;
    private readonly TimeZoneInfo _timeZone;

    /// <summary>
    /// Original UTC prayer times
    /// </summary>
    public PrayerTimes UtcPrayerTimes => _utcPrayerTimes;

    /// <summary>
    /// Timezone ID used for conversion
    /// </summary>
    public string TimeZoneId => _timeZoneId;

    /// <summary>
    /// Fajr prayer time in local timezone
    /// </summary>
    public DateTime Fajr => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Fajr, _timeZone);

    /// <summary>
    /// Sunrise time in local timezone
    /// </summary>
    public DateTime Sunrise => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Sunrise, _timeZone);

    /// <summary>
    /// Dhuhr prayer time in local timezone
    /// </summary>
    public DateTime Dhuhr => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Dhuhr, _timeZone);

    /// <summary>
    /// Asr prayer time in local timezone
    /// </summary>
    public DateTime Asr => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Asr, _timeZone);

    /// <summary>
    /// Sunset time in local timezone
    /// </summary>
    public DateTime Sunset => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Sunset, _timeZone);

    /// <summary>
    /// Maghrib prayer time in local timezone
    /// </summary>
    public DateTime Maghrib => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Maghrib, _timeZone);

    /// <summary>
    /// Isha prayer time in local timezone
    /// </summary>
    public DateTime Isha => TimeZoneHelper.ConvertToTimeZone(_utcPrayerTimes.Isha, _timeZone);

    /// <summary>
    /// Coordinates used for calculation
    /// </summary>
    public Coordinates Coordinates => _utcPrayerTimes.Coordinates;

    /// <summary>
    /// Date used for calculation
    /// </summary>
    public DateTime Date => _utcPrayerTimes.Date;

    /// <summary>
    /// Calculation parameters used
    /// </summary>
    public CalculationParameters CalculationParameters => _utcPrayerTimes.CalculationParameters;

    /// <summary>
    /// Whether daylight saving time is in effect for this date
    /// </summary>
    public bool IsDaylightSavingTime => TimeZoneHelper.IsDaylightSavingTime(Date, _timeZoneId);

    /// <summary>
    /// Current timezone offset from UTC (including DST)
    /// </summary>
    public TimeSpan TimezoneOffset => TimeZoneHelper.GetTimezoneOffset(Date, _timeZoneId);

    /// <summary>
    /// Initializes prayer times with timezone conversion
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <param name="date">Date for prayer times</param>
    /// <param name="calculationParameters">Calculation parameters</param>
    /// <param name="timeZoneId">IANA timezone ID (e.g., "America/New_York")</param>
    public PrayerTimesWithTimeZone(Coordinates coordinates, DateTime date, 
        CalculationParameters calculationParameters, string timeZoneId)
    {
        _utcPrayerTimes = new PrayerTimes(coordinates, date, calculationParameters);
        _timeZoneId = timeZoneId;
        
        try
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneHelper.ConvertIanaToWindows(timeZoneId));
        }
        catch
        {
            try
            {
                _timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch
            {
                _timeZone = TimeZoneInfo.Utc;
            }
        }
    }

    /// <summary>
    /// Initializes prayer times with TimeZoneInfo
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <param name="date">Date for prayer times</param>
    /// <param name="calculationParameters">Calculation parameters</param>
    /// <param name="timeZone">TimeZoneInfo object</param>
    public PrayerTimesWithTimeZone(Coordinates coordinates, DateTime date, 
        CalculationParameters calculationParameters, TimeZoneInfo timeZone)
    {
        _utcPrayerTimes = new PrayerTimes(coordinates, date, calculationParameters);
        _timeZone = timeZone;
        _timeZoneId = timeZone.Id;
    }

    /// <summary>
    /// Gets the time for a specific prayer in local timezone
    /// </summary>
    /// <param name="prayer">The prayer to get time for</param>
    /// <returns>Prayer time in local timezone, or null if prayer is None</returns>
    public DateTime? TimeForPrayer(Prayer prayer)
    {
        var utcTime = _utcPrayerTimes.TimeForPrayer(prayer);
        return utcTime.HasValue ? TimeZoneHelper.ConvertToTimeZone(utcTime.Value, _timeZone) : null;
    }

    /// <summary>
    /// Gets the current prayer for the given time in local timezone
    /// </summary>
    /// <param name="time">Time to check (defaults to current local time)</param>
    /// <returns>Current prayer</returns>
    public Prayer CurrentPrayer(DateTime? time = null)
    {
        var checkTime = time ?? DateTime.Now;
        
        // Convert local time to UTC for comparison
        var utcTime = TimeZoneInfo.ConvertTimeToUtc(checkTime, _timeZone);
        
        return _utcPrayerTimes.CurrentPrayer(utcTime);
    }

    /// <summary>
    /// Gets the next prayer for the given time in local timezone
    /// </summary>
    /// <param name="time">Time to check (defaults to current local time)</param>
    /// <returns>Next prayer</returns>
    public Prayer NextPrayer(DateTime? time = null)
    {
        var checkTime = time ?? DateTime.Now;
        
        // Convert local time to UTC for comparison
        var utcTime = TimeZoneInfo.ConvertTimeToUtc(checkTime, _timeZone);
        
        return _utcPrayerTimes.NextPrayer(utcTime);
    }

    /// <summary>
    /// Formats a prayer time with timezone information
    /// </summary>
    /// <param name="prayer">Prayer to format</param>
    /// <param name="format">Format string (default: "HH:mm")</param>
    /// <returns>Formatted time string with timezone info</returns>
    public string FormatPrayerTime(Prayer prayer, string format = "HH:mm")
    {
        var time = TimeForPrayer(prayer);
        if (!time.HasValue || time.Value == DateTime.MinValue)
            return "N/A";

        var timeStr = time.Value.ToString(format);
        var offsetStr = $"UTC{TimezoneOffset.Hours:+0;-0}:{Math.Abs(TimezoneOffset.Minutes):D2}";
        var dstStr = IsDaylightSavingTime ? " (DST)" : "";
        
        return $"{timeStr} {offsetStr}{dstStr}";
    }

    /// <summary>
    /// Gets timezone information summary
    /// </summary>
    /// <returns>Timezone info string</returns>
    public string GetTimezoneInfo()
    {
        var offsetStr = $"UTC{TimezoneOffset.Hours:+0;-0}:{Math.Abs(TimezoneOffset.Minutes):D2}";
        var dstStr = IsDaylightSavingTime ? " (Daylight Saving Time)" : " (Standard Time)";
        return $"{_timeZone.DisplayName} - {offsetStr}{dstStr}";
    }
}
