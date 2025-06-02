namespace AzanDotNet;

/// <summary>
/// Represents time components (hours and minutes) and provides conversion to DateTime
/// </summary>
internal class TimeComponents
{
    /// <summary>
    /// Hours component
    /// </summary>
    public double Hours { get; }
    
    /// <summary>
    /// Minutes component
    /// </summary>
    public double Minutes { get; }

    /// <summary>
    /// Seconds component
    /// </summary>
    public double Seconds { get; }

    /// <summary>
    /// Initializes a new instance of TimeComponents from decimal hours
    /// </summary>
    /// <param name="decimalHours">Time in decimal hours (e.g., 14.5 for 2:30 PM)</param>
    public TimeComponents(double decimalHours)
    {
        if (double.IsNaN(decimalHours) || double.IsInfinity(decimalHours))
        {
            Hours = double.NaN;
            Minutes = double.NaN;
            Seconds = double.NaN;
        }
        else
        {
            Hours = Math.Floor(decimalHours);
            Minutes = Math.Floor((decimalHours - Hours) * 60.0);
            Seconds = Math.Floor((decimalHours - (Hours + Minutes / 60.0)) * 3600.0);
        }
    }

    /// <summary>
    /// Initializes a new instance of TimeComponents from hours and minutes
    /// </summary>
    /// <param name="hours">Hours component</param>
    /// <param name="minutes">Minutes component</param>
    /// <param name="seconds">Seconds component</param>
    public TimeComponents(double hours, double minutes, double seconds = 0)
    {
        Hours = hours;
        Minutes = minutes;
        Seconds = seconds;
    }

    /// <summary>
    /// Converts the time components to a UTC DateTime for the specified date
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month (1-12, C# style)</param>
    /// <param name="day">Day of month</param>
    /// <returns>UTC DateTime</returns>
    public DateTime UtcDate(int year, int month, int day)
    {
        // Handle invalid values
        if (double.IsNaN(Hours) || double.IsInfinity(Hours) ||
            double.IsNaN(Minutes) || double.IsInfinity(Minutes) ||
            double.IsNaN(Seconds) || double.IsInfinity(Seconds))
        {
            return DateTime.MinValue;
        }

        try
        {
            // Handle hours >= 24 by adding days
            var totalHours = Hours;
            var daysToAdd = 0;

            while (totalHours >= 24)
            {
                totalHours -= 24;
                daysToAdd++;
            }

            while (totalHours < 0)
            {
                totalHours += 24;
                daysToAdd--;
            }

            var baseDate = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            var adjustedDate = baseDate.AddDays(daysToAdd);

            return new DateTime(adjustedDate.Year, adjustedDate.Month, adjustedDate.Day,
                               (int)totalHours, (int)Minutes, (int)Seconds, DateTimeKind.Utc);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Converts the time components to a local DateTime for the specified date and timezone
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month (1-12, C# style)</param>
    /// <param name="day">Day of month</param>
    /// <param name="timeZoneInfo">Timezone information (null for system local time)</param>
    /// <returns>Local DateTime</returns>
    public DateTime LocalDate(int year, int month, int day, TimeZoneInfo? timeZoneInfo = null)
    {
        var utcDateTime = UtcDate(year, month, day);
        if (utcDateTime == DateTime.MinValue)
        {
            return DateTime.MinValue;
        }

        var targetTimeZone = timeZoneInfo ?? TimeZoneInfo.Local;
        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, targetTimeZone);
    }

    /// <summary>
    /// Returns a string representation of the time components
    /// </summary>
    /// <returns>String in format "HH:MM"</returns>
    public override string ToString()
    {
        var h = (int)Hours;
        var m = (int)Math.Round(Minutes);
        return $"{h:D2}:{m:D2}";
    }
}
