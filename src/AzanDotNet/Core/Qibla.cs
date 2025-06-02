namespace AzanDotNet;

/// <summary>
/// Qibla direction calculation
/// </summary>
public static class Qibla
{
    /// <summary>
    /// Coordinates of the Kaaba in Makkah
    /// </summary>
    private static readonly Coordinates Makkah = new(21.4225241, 39.8261818);

    /// <summary>
    /// Calculates the Qibla direction from the given coordinates
    /// </summary>
    /// <param name="coordinates">Observer coordinates</param>
    /// <returns>Qibla direction in degrees from North</returns>
    public static double GetDirection(Coordinates coordinates)
    {
        // Special case: if we're at or very close to Makkah, return 0
        var latDiff = Math.Abs(coordinates.Latitude - Makkah.Latitude);
        var lonDiff = Math.Abs(coordinates.Longitude - Makkah.Longitude);

        if (latDiff < 0.01 && lonDiff < 0.01) // Within ~1km of Kaaba
        {
            return 0.0;
        }

        // Convert coordinates to radians
        var lat1 = MathUtils.DegreesToRadians(coordinates.Latitude);
        var lon1 = MathUtils.DegreesToRadians(coordinates.Longitude);
        var lat2 = MathUtils.DegreesToRadians(Makkah.Latitude);
        var lon2 = MathUtils.DegreesToRadians(Makkah.Longitude);

        // Calculate the difference in longitude
        var deltaLon = lon2 - lon1;

        // Calculate the bearing using the great circle formula
        var y = Math.Sin(deltaLon) * Math.Cos(lat2);
        var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon);

        var bearing = Math.Atan2(y, x);

        // Convert to degrees and normalize to 0-360
        var bearingDegrees = MathUtils.RadiansToDegrees(bearing);
        return MathUtils.UnwindAngle(bearingDegrees);
    }
}
