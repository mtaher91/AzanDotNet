namespace AzanDotNet;

/// <summary>
/// Date and time utility functions
/// </summary>
internal static class DateUtils
{
    /// <summary>
    /// Adds the specified number of days to a date
    /// </summary>
    /// <param name="date">The original date</param>
    /// <param name="days">Number of days to add</param>
    /// <returns>New date with days added</returns>
    public static DateTime AddDays(DateTime date, int days)
    {
        return date.AddDays(days);
    }

    /// <summary>
    /// Adds the specified number of minutes to a date
    /// </summary>
    /// <param name="date">The original date</param>
    /// <param name="minutes">Number of minutes to add</param>
    /// <returns>New date with minutes added</returns>
    public static DateTime AddMinutes(DateTime date, double minutes)
    {
        if (date == DateTime.MinValue || double.IsNaN(minutes) || double.IsInfinity(minutes))
        {
            return DateTime.MinValue;
        }

        try
        {
            return date.AddMinutes(minutes);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Adds the specified number of seconds to a date
    /// </summary>
    /// <param name="date">The original date</param>
    /// <param name="seconds">Number of seconds to add</param>
    /// <returns>New date with seconds added</returns>
    public static DateTime AddSeconds(DateTime date, double seconds)
    {
        if (date == DateTime.MinValue || double.IsNaN(seconds) || double.IsInfinity(seconds))
        {
            return DateTime.MinValue;
        }

        try
        {
            return date.AddSeconds(seconds);
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    /// <summary>
    /// Gets the day of year for the specified date
    /// </summary>
    /// <param name="date">The date</param>
    /// <returns>Day of year (1-366)</returns>
    public static int DayOfYear(DateTime date)
    {
        return date.DayOfYear;
    }

    /// <summary>
    /// Validates if a date is valid
    /// </summary>
    /// <param name="date">The date to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static bool IsValidDate(DateTime date)
    {
        return date != DateTime.MinValue && date != DateTime.MaxValue;
    }

    /// <summary>
    /// Rounds a DateTime to the nearest minute based on the rounding option
    /// </summary>
    /// <param name="dateTime">The DateTime to round</param>
    /// <param name="rounding">The rounding option</param>
    /// <returns>Rounded DateTime</returns>
    public static DateTime RoundedMinute(DateTime dateTime, Rounding rounding = Rounding.Nearest)
    {
        return rounding switch
        {
            Rounding.Nearest => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 
                dateTime.Hour, dateTime.Minute, 0, dateTime.Kind)
                .AddMinutes(dateTime.Second >= 30 ? 1 : 0),
            
            Rounding.Up => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 
                dateTime.Hour, dateTime.Minute, 0, dateTime.Kind)
                .AddMinutes(dateTime.Second > 0 || dateTime.Millisecond > 0 ? 1 : 0),
            
            Rounding.None => dateTime,
            
            _ => dateTime
        };
    }
}
