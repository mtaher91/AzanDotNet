namespace AzanDotNet;

/// <summary>
/// Prayer times for a given date and location
/// </summary>
public class PrayerTimes
{
    /// <summary>
    /// Fajr prayer time
    /// </summary>
    public DateTime Fajr { get; }

    /// <summary>
    /// Sunrise time (not a prayer, but calculated for reference)
    /// </summary>
    public DateTime Sunrise { get; }

    /// <summary>
    /// Dhuhr prayer time
    /// </summary>
    public DateTime Dhuhr { get; }

    /// <summary>
    /// Asr prayer time
    /// </summary>
    public DateTime Asr { get; }

    /// <summary>
    /// Sunset time
    /// </summary>
    public DateTime Sunset { get; }

    /// <summary>
    /// Maghrib prayer time
    /// </summary>
    public DateTime Maghrib { get; }

    /// <summary>
    /// Isha prayer time
    /// </summary>
    public DateTime Isha { get; }

    /// <summary>
    /// Coordinates used for calculation
    /// </summary>
    public Coordinates Coordinates { get; }

    /// <summary>
    /// Date used for calculation
    /// </summary>
    public DateTime Date { get; }

    /// <summary>
    /// Calculation parameters used
    /// </summary>
    public CalculationParameters CalculationParameters { get; }

    /// <summary>
    /// Timezone used for calculations (null means UTC)
    /// </summary>
    public TimeZoneInfo? TimeZone { get; }

    /// <summary>
    /// Initializes prayer times for the given coordinates, date, and calculation parameters
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <param name="date">Date for prayer times</param>
    /// <param name="calculationParameters">Calculation parameters</param>
    public PrayerTimes(Coordinates coordinates, DateTime date, CalculationParameters calculationParameters)
        : this(coordinates, date, calculationParameters, null)
    {
    }

    /// <summary>
    /// Initializes prayer times for the given coordinates, date, calculation parameters, and timezone
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <param name="date">Date for prayer times</param>
    /// <param name="calculationParameters">Calculation parameters</param>
    /// <param name="timeZone">Timezone for the results (null for UTC)</param>
    public PrayerTimes(Coordinates coordinates, DateTime date, CalculationParameters calculationParameters, TimeZoneInfo? timeZone)
    {
        Coordinates = coordinates;
        Date = date.Date; // Ensure we only use the date part
        CalculationParameters = calculationParameters;
        TimeZone = timeZone;

        var solarTime = new SolarTime(Date, coordinates);

        DateTime fajrTime;
        DateTime sunriseTime;
        DateTime dhuhrTime;
        DateTime asrTime;
        DateTime sunsetTime;
        DateTime maghribTime;
        DateTime ishaTime;
        double nightFraction;

        dhuhrTime = ConvertToDateTime(new TimeComponents(solarTime.Transit), Date.Year, Date.Month, Date.Day);
        sunriseTime = ConvertToDateTime(new TimeComponents(solarTime.Sunrise), Date.Year, Date.Month, Date.Day);
        sunsetTime = ConvertToDateTime(new TimeComponents(solarTime.Sunset), Date.Year, Date.Month, Date.Day);

        var tomorrow = Date.AddDays(1);
        var tomorrowSolarTime = new SolarTime(tomorrow, coordinates);

        // Handle polar circle resolution
        if ((!IsValidDateTime(sunriseTime) || !IsValidDateTime(sunsetTime) ||
             double.IsNaN(tomorrowSolarTime.Sunrise)) &&
            calculationParameters.PolarCircleResolution != PolarCircleResolution.Unresolved)
        {
            var resolved = ResolvePolarCircle(calculationParameters.PolarCircleResolution, Date, coordinates);
            solarTime = resolved.SolarTime;
            tomorrowSolarTime = resolved.TomorrowSolarTime;

            dhuhrTime = ConvertToDateTime(new TimeComponents(solarTime.Transit), Date.Year, Date.Month, Date.Day);
            sunriseTime = ConvertToDateTime(new TimeComponents(solarTime.Sunrise), Date.Year, Date.Month, Date.Day);
            sunsetTime = ConvertToDateTime(new TimeComponents(solarTime.Sunset), Date.Year, Date.Month, Date.Day);
        }

        asrTime = ConvertToDateTime(new TimeComponents(solarTime.Afternoon(calculationParameters.Madhab.GetShadowLength())), Date.Year, Date.Month, Date.Day);

        var tomorrowSunrise = ConvertToDateTime(new TimeComponents(tomorrowSolarTime.Sunrise), tomorrow.Year, tomorrow.Month, tomorrow.Day);
        var night = tomorrowSunrise.Subtract(sunsetTime).TotalSeconds;

        fajrTime = ConvertToDateTime(new TimeComponents(solarTime.HourAngle(-calculationParameters.FajrAngle, false)), Date.Year, Date.Month, Date.Day);

        // Special case for Moonsighting Committee above 55° latitude
        if (calculationParameters.Method == "MoonsightingCommittee" && coordinates.Latitude >= 55)
        {
            nightFraction = night / 7.0;
            fajrTime = sunriseTime.AddSeconds(-nightFraction);
        }

        // Calculate safe Fajr time
        DateTime safeFajr;
        if (calculationParameters.Method == "MoonsightingCommittee")
        {
            safeFajr = Astronomical.SeasonAdjustedMorningTwilight(
                coordinates.Latitude, Date.DayOfYear, Date.Year, sunriseTime);
        }
        else
        {
            var nightPortions = calculationParameters.GetNightPortions();
            nightFraction = nightPortions.FajrPortion * night;
            safeFajr = sunriseTime.AddSeconds(-nightFraction);
        }

        // Use safe Fajr if calculated Fajr is invalid or later than safe Fajr
        if (!IsValidDateTime(fajrTime) || safeFajr > fajrTime)
        {
            fajrTime = safeFajr;
        }

        if (calculationParameters.IshaInterval > 0)
        {
            ishaTime = sunsetTime.AddMinutes(calculationParameters.IshaInterval);
        }
        else
        {
            ishaTime = ConvertToDateTime(new TimeComponents(solarTime.HourAngle(-calculationParameters.IshaAngle, true)), Date.Year, Date.Month, Date.Day);

            // Special case for Moonsighting Committee above 55° latitude
            if (calculationParameters.Method == "MoonsightingCommittee" && coordinates.Latitude >= 55)
            {
                nightFraction = night / 7.0;
                ishaTime = sunsetTime.AddSeconds(nightFraction);
            }

            // Calculate safe Isha time
            DateTime safeIsha;
            if (calculationParameters.Method == "MoonsightingCommittee")
            {
                safeIsha = Astronomical.SeasonAdjustedEveningTwilight(
                    coordinates.Latitude, Date.DayOfYear, Date.Year, sunsetTime, Shafaq.General);
            }
            else
            {
                var nightPortions = calculationParameters.GetNightPortions();
                nightFraction = nightPortions.IshaPortion * night;
                safeIsha = sunsetTime.AddSeconds(nightFraction);
            }

            // Use safe Isha if calculated Isha is invalid or earlier than safe Isha
            if (!IsValidDateTime(ishaTime) || safeIsha < ishaTime)
            {
                ishaTime = safeIsha;
            }
        }

        maghribTime = sunsetTime;
        if (calculationParameters.MaghribAngle > 0)
        {
            var angleBasedMaghrib = ConvertToDateTime(new TimeComponents(solarTime.HourAngle(-calculationParameters.MaghribAngle, true)), Date.Year, Date.Month, Date.Day);

            // Only use angle-based maghrib if it's between sunset and isha
            if (IsValidDateTime(angleBasedMaghrib) &&
                sunsetTime < angleBasedMaghrib &&
                IsValidDateTime(ishaTime) &&
                ishaTime > angleBasedMaghrib)
            {
                maghribTime = angleBasedMaghrib;
            }
        }

        // Apply high latitude rule adjustments
        if (Math.Abs(coordinates.Latitude) > 48)
        {
            var nightPortions = calculationParameters.GetNightPortions();

            // Check if fajr time is invalid or after sunrise
            if (fajrTime == DateTime.MinValue || fajrTime >= sunriseTime)
            {
                fajrTime = sunriseTime.AddSeconds(-night * nightPortions.FajrPortion);
            }

            // Check if isha time is invalid or before sunset
            if (ishaTime == DateTime.MinValue || ishaTime <= sunsetTime)
            {
                ishaTime = sunsetTime.AddSeconds(night * nightPortions.IshaPortion);
            }
        }

        // Apply adjustments
        var fajrAdjustment = (calculationParameters.Adjustments.Fajr + calculationParameters.MethodAdjustments.Fajr);
        var sunriseAdjustment = (calculationParameters.Adjustments.Sunrise + calculationParameters.MethodAdjustments.Sunrise);
        var dhuhrAdjustment = (calculationParameters.Adjustments.Dhuhr + calculationParameters.MethodAdjustments.Dhuhr);
        var asrAdjustment = (calculationParameters.Adjustments.Asr + calculationParameters.MethodAdjustments.Asr);
        var maghribAdjustment = (calculationParameters.Adjustments.Maghrib + calculationParameters.MethodAdjustments.Maghrib);
        var ishaAdjustment = (calculationParameters.Adjustments.Isha + calculationParameters.MethodAdjustments.Isha);

        Fajr = DateUtils.RoundedMinute(DateUtils.AddMinutes(fajrTime, fajrAdjustment), calculationParameters.Rounding);
        Sunrise = DateUtils.RoundedMinute(DateUtils.AddMinutes(sunriseTime, sunriseAdjustment), calculationParameters.Rounding);
        Dhuhr = DateUtils.RoundedMinute(DateUtils.AddMinutes(dhuhrTime, dhuhrAdjustment), calculationParameters.Rounding);
        Asr = DateUtils.RoundedMinute(DateUtils.AddMinutes(asrTime, asrAdjustment), calculationParameters.Rounding);
        Sunset = DateUtils.RoundedMinute(sunsetTime, calculationParameters.Rounding);
        Maghrib = DateUtils.RoundedMinute(DateUtils.AddMinutes(maghribTime, maghribAdjustment), calculationParameters.Rounding);
        Isha = DateUtils.RoundedMinute(DateUtils.AddMinutes(ishaTime, ishaAdjustment), calculationParameters.Rounding);
    }

    /// <summary>
    /// Gets the time for a specific prayer
    /// </summary>
    /// <param name="prayer">The prayer to get time for</param>
    /// <returns>Prayer time, or null if prayer is None</returns>
    public DateTime? TimeForPrayer(Prayer prayer)
    {
        return prayer switch
        {
            Prayer.Fajr => Fajr,
            Prayer.Sunrise => Sunrise,
            Prayer.Dhuhr => Dhuhr,
            Prayer.Asr => Asr,
            Prayer.Maghrib => Maghrib,
            Prayer.Isha => Isha,
            _ => null
        };
    }

    /// <summary>
    /// Gets the current prayer for the given time
    /// </summary>
    /// <param name="time">Time to check (defaults to current time)</param>
    /// <returns>Current prayer</returns>
    public Prayer CurrentPrayer(DateTime? time = null)
    {
        var checkTime = time ?? DateTime.UtcNow;
        
        if (checkTime >= Isha)
            return Prayer.Isha;
        else if (checkTime >= Maghrib)
            return Prayer.Maghrib;
        else if (checkTime >= Asr)
            return Prayer.Asr;
        else if (checkTime >= Dhuhr)
            return Prayer.Dhuhr;
        else if (checkTime >= Sunrise)
            return Prayer.Sunrise;
        else if (checkTime >= Fajr)
            return Prayer.Fajr;
        else
            return Prayer.None;
    }

    /// <summary>
    /// Gets the next prayer for the given time
    /// </summary>
    /// <param name="time">Time to check (defaults to current time)</param>
    /// <returns>Next prayer</returns>
    public Prayer NextPrayer(DateTime? time = null)
    {
        var checkTime = time ?? DateTime.UtcNow;
        
        if (checkTime >= Isha)
            return Prayer.None;
        else if (checkTime >= Maghrib)
            return Prayer.Isha;
        else if (checkTime >= Asr)
            return Prayer.Maghrib;
        else if (checkTime >= Dhuhr)
            return Prayer.Asr;
        else if (checkTime >= Sunrise)
            return Prayer.Dhuhr;
        else if (checkTime >= Fajr)
            return Prayer.Sunrise;
        else
            return Prayer.Fajr;
    }

    /// <summary>
    /// Converts TimeComponents to DateTime using the appropriate timezone
    /// </summary>
    /// <param name="timeComponents">Time components to convert</param>
    /// <param name="year">Year</param>
    /// <param name="month">Month</param>
    /// <param name="day">Day</param>
    /// <returns>DateTime in the appropriate timezone</returns>
    private DateTime ConvertToDateTime(TimeComponents timeComponents, int year, int month, int day)
    {
        return TimeZone != null
            ? timeComponents.LocalDate(year, month, day, TimeZone)
            : timeComponents.UtcDate(year, month, day);
    }

    /// <summary>
    /// Checks if a DateTime is valid (not MinValue or MaxValue)
    /// </summary>
    /// <param name="dateTime">DateTime to check</param>
    /// <returns>True if valid</returns>
    private static bool IsValidDateTime(DateTime dateTime)
    {
        return dateTime != DateTime.MinValue && dateTime != DateTime.MaxValue;
    }

    /// <summary>
    /// Resolves polar circle issues by finding the nearest valid date
    /// </summary>
    /// <param name="resolution">Polar circle resolution method</param>
    /// <param name="date">Current date</param>
    /// <param name="coordinates">Location coordinates</param>
    /// <returns>Resolved solar times</returns>
    private static (SolarTime SolarTime, SolarTime TomorrowSolarTime) ResolvePolarCircle(
        PolarCircleResolution resolution, DateTime date, Coordinates coordinates)
    {
        if (resolution == PolarCircleResolution.Unresolved)
        {
            throw new ArgumentException("Cannot resolve polar circle with Unresolved method");
        }

        var daysToAdd = resolution == PolarCircleResolution.AqrabBalad ? 1 : 7;
        var direction = 1;

        // Try both directions to find valid dates
        for (var i = 1; i <= 365; i++)
        {
            var testDate = date.AddDays(i * direction * daysToAdd);
            var testSolarTime = new SolarTime(testDate, coordinates);
            var testTomorrowSolarTime = new SolarTime(testDate.AddDays(1), coordinates);

            if (!double.IsNaN(testSolarTime.Sunrise) && !double.IsNaN(testSolarTime.Sunset) &&
                !double.IsNaN(testTomorrowSolarTime.Sunrise))
            {
                return (testSolarTime, testTomorrowSolarTime);
            }

            // Switch direction
            direction *= -1;
        }

        // Fallback to original if no valid date found
        return (new SolarTime(date, coordinates), new SolarTime(date.AddDays(1), coordinates));
    }
}
