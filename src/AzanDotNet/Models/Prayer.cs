namespace AzanDotNet;

/// <summary>
/// Represents the different Islamic prayers
/// </summary>
public enum Prayer
{
    /// <summary>
    /// No prayer
    /// </summary>
    None,
    
    /// <summary>
    /// Fajr prayer (dawn)
    /// </summary>
    Fajr,
    
    /// <summary>
    /// Sunrise (not a prayer, but calculated for reference)
    /// </summary>
    Sunrise,
    
    /// <summary>
    /// Dhuhr prayer (midday)
    /// </summary>
    Dhuhr,
    
    /// <summary>
    /// Asr prayer (afternoon)
    /// </summary>
    Asr,
    
    /// <summary>
    /// Maghrib prayer (sunset)
    /// </summary>
    Maghrib,
    
    /// <summary>
    /// Isha prayer (night)
    /// </summary>
    Isha
}
