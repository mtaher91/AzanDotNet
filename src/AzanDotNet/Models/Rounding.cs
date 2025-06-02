namespace AzanDotNet;

/// <summary>
/// Rounding options for prayer times
/// </summary>
public enum Rounding
{
    /// <summary>
    /// Round to the nearest minute
    /// </summary>
    Nearest,
    
    /// <summary>
    /// Round up to the next minute
    /// </summary>
    Up,
    
    /// <summary>
    /// No rounding - keep exact time
    /// </summary>
    None
}
