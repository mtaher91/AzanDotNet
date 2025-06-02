namespace AzanDotNet;

/// <summary>
/// Parameters for calculating prayer times
/// </summary>
public class CalculationParameters
{
    /// <summary>
    /// Name of the calculation method
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// Angle of the sun below the horizon used for calculating Fajr
    /// </summary>
    public double FajrAngle { get; set; }

    /// <summary>
    /// Angle of the sun below the horizon used for calculating Isha
    /// </summary>
    public double IshaAngle { get; set; }

    /// <summary>
    /// Minutes after Maghrib to determine time for Isha
    /// If this value is greater than 0, then IshaAngle is not used
    /// </summary>
    public double IshaInterval { get; set; }

    /// <summary>
    /// Angle of the sun below the horizon used for calculating Maghrib
    /// Only used by the Tehran method to account for lightness in the sky
    /// </summary>
    public double MaghribAngle { get; set; }

    /// <summary>
    /// Madhab to determine how Asr is calculated
    /// </summary>
    public Madhab Madhab { get; set; } = Madhab.Shafi;

    /// <summary>
    /// Rule to determine the earliest time for Fajr and latest time for Isha
    /// needed for high latitude locations
    /// </summary>
    public HighLatitudeRule HighLatitudeRule { get; set; } = HighLatitudeRule.MiddleOfTheNight;

    /// <summary>
    /// Resolution method for polar circle regions
    /// </summary>
    public PolarCircleResolution PolarCircleResolution { get; set; } = PolarCircleResolution.Unresolved;

    /// <summary>
    /// Rounding option for prayer times
    /// </summary>
    public Rounding Rounding { get; set; } = Rounding.Nearest;

    /// <summary>
    /// Type of twilight used for calculating Isha
    /// </summary>
    public Shafaq Shafaq { get; set; } = Shafaq.General;

    /// <summary>
    /// Manual adjustments (in minutes) to be added to each prayer time
    /// </summary>
    public PrayerAdjustments Adjustments { get; set; } = new();

    /// <summary>
    /// Adjustments set by a calculation method. This value should not be manually modified
    /// </summary>
    public PrayerAdjustments MethodAdjustments { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of CalculationParameters
    /// </summary>
    /// <param name="method">Name of the calculation method</param>
    /// <param name="fajrAngle">Fajr angle</param>
    /// <param name="ishaAngle">Isha angle</param>
    /// <param name="ishaInterval">Isha interval in minutes</param>
    /// <param name="maghribAngle">Maghrib angle</param>
    public CalculationParameters(string? method = null, double fajrAngle = 0, double ishaAngle = 0, 
        double ishaInterval = 0, double maghribAngle = 0)
    {
        Method = method ?? "Other";
        FajrAngle = fajrAngle;
        IshaAngle = ishaAngle;
        IshaInterval = ishaInterval;
        MaghribAngle = maghribAngle;
    }

    /// <summary>
    /// Gets the night portions for high latitude rule calculations
    /// </summary>
    /// <returns>Tuple of (fajr portion, isha portion)</returns>
    public (double FajrPortion, double IshaPortion) GetNightPortions()
    {
        return HighLatitudeRule switch
        {
            HighLatitudeRule.MiddleOfTheNight => (0.5, 0.5),
            HighLatitudeRule.SeventhOfTheNight => (1.0 / 7.0, 1.0 / 7.0),
            HighLatitudeRule.TwilightAngle => (FajrAngle / 60.0, IshaAngle / 60.0),
            _ => (0.5, 0.5)
        };
    }
}

/// <summary>
/// Prayer time adjustments in minutes
/// </summary>
public class PrayerAdjustments
{
    /// <summary>
    /// Fajr adjustment in minutes
    /// </summary>
    public double Fajr { get; set; }

    /// <summary>
    /// Sunrise adjustment in minutes
    /// </summary>
    public double Sunrise { get; set; }

    /// <summary>
    /// Dhuhr adjustment in minutes
    /// </summary>
    public double Dhuhr { get; set; }

    /// <summary>
    /// Asr adjustment in minutes
    /// </summary>
    public double Asr { get; set; }

    /// <summary>
    /// Maghrib adjustment in minutes
    /// </summary>
    public double Maghrib { get; set; }

    /// <summary>
    /// Isha adjustment in minutes
    /// </summary>
    public double Isha { get; set; }
}
