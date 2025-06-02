namespace AzanDotNet;

/// <summary>
/// Rules for resolving prayer times in polar circle regions
/// </summary>
public enum PolarCircleResolution
{
    /// <summary>
    /// Use the closest day that has valid prayer times
    /// </summary>
    AqrabYaum,
    
    /// <summary>
    /// Use the closest balanced day (day and night are roughly equal)
    /// </summary>
    AqrabBalad,
    
    /// <summary>
    /// No resolution - return invalid times
    /// </summary>
    Unresolved
}
