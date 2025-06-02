namespace AzanDotNet;

/// <summary>
/// Represents the different Islamic schools of jurisprudence (madhabs)
/// Used to determine how Asr prayer time is calculated
/// </summary>
public enum Madhab
{
    /// <summary>
    /// Shafi madhab - shadow length equals object height (1:1 ratio)
    /// </summary>
    Shafi,
    
    /// <summary>
    /// Hanafi madhab - shadow length equals twice the object height (2:1 ratio)
    /// </summary>
    Hanafi
}

/// <summary>
/// Utility methods for Madhab calculations
/// </summary>
public static class MadhabExtensions
{
    /// <summary>
    /// Gets the shadow length multiplier for the given madhab
    /// </summary>
    /// <param name="madhab">The madhab to get shadow length for</param>
    /// <returns>Shadow length multiplier (1 for Shafi, 2 for Hanafi)</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid madhab is provided</exception>
    public static double GetShadowLength(this Madhab madhab)
    {
        return madhab switch
        {
            Madhab.Shafi => 1.0,
            Madhab.Hanafi => 2.0,
            _ => throw new ArgumentException($"Invalid Madhab: {madhab}", nameof(madhab))
        };
    }
}
