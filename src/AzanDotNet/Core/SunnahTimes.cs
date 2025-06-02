namespace AzanDotNet;

/// <summary>
/// Sunnah times calculated from prayer times
/// </summary>
public class SunnahTimes
{
    /// <summary>
    /// Middle of the night time
    /// </summary>
    public DateTime MiddleOfTheNight { get; }

    /// <summary>
    /// Last third of the night time
    /// </summary>
    public DateTime LastThirdOfTheNight { get; }

    /// <summary>
    /// Initializes Sunnah times from prayer times
    /// </summary>
    /// <param name="prayerTimes">Prayer times to calculate from</param>
    public SunnahTimes(PrayerTimes prayerTimes)
    {
        var date = prayerTimes.Date;
        var nextDay = DateUtils.AddDays(date, 1);
        var nextDayPrayerTimes = new PrayerTimes(
            prayerTimes.Coordinates,
            nextDay,
            prayerTimes.CalculationParameters);

        var nightDuration = nextDayPrayerTimes.Fajr.Subtract(prayerTimes.Maghrib).TotalSeconds;

        MiddleOfTheNight = DateUtils.RoundedMinute(
            DateUtils.AddSeconds(prayerTimes.Maghrib, nightDuration / 2));

        LastThirdOfTheNight = DateUtils.RoundedMinute(
            DateUtils.AddSeconds(prayerTimes.Maghrib, nightDuration * (2.0 / 3.0)));
    }
}
