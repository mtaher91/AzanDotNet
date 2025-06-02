namespace AzanDotNet;

/// <summary>
/// Rules for calculating prayer times in high latitude locations
/// where Fajr and Isha may not truly exist or may present a hardship
/// </summary>
public enum HighLatitudeRule
{
    /// <summary>
    /// Fajr will never be earlier than the middle of the night
    /// and Isha will never be later than the middle of the night
    /// </summary>
    MiddleOfTheNight,
    
    /// <summary>
    /// Fajr will never be earlier than the beginning of the seventh of the night
    /// and Isha will never be later than the end of the seventh of the night
    /// </summary>
    SeventhOfTheNight,
    
    /// <summary>
    /// Similar to SeventhOfTheNight, but instead of 1/7, use the angle specified
    /// by fajrAngle and ishaAngle as a fraction of the night
    /// </summary>
    TwilightAngle
}
