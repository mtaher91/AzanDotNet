namespace AzanDotNet;

/// <summary>
/// Mathematical utility functions for astronomical calculations
/// </summary>
internal static class MathUtils
{
    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="degrees">Angle in degrees</param>
    /// <returns>Angle in radians</returns>
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="radians">Angle in radians</param>
    /// <returns>Angle in degrees</returns>
    public static double RadiansToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }

    /// <summary>
    /// Normalizes an angle to be within the range [0, 360) degrees
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns>Normalized angle in degrees</returns>
    public static double UnwindAngle(double angle)
    {
        return angle - 360.0 * Math.Floor(angle / 360.0);
    }

    /// <summary>
    /// Normalizes an angle to be within a specified scale
    /// </summary>
    /// <param name="angle">Angle to normalize</param>
    /// <param name="max">Maximum value of the scale</param>
    /// <returns>Normalized angle</returns>
    public static double NormalizeToScale(double angle, double max)
    {
        return angle - max * Math.Floor(angle / max);
    }

    /// <summary>
    /// Adjusts an angle to the correct quadrant
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns>Adjusted angle</returns>
    public static double QuadrantShiftAngle(double angle)
    {
        if (angle >= -180 && angle <= 180)
        {
            return angle;
        }

        return angle - 360 * Math.Round(angle / 360);
    }

    /// <summary>
    /// Calculates the closest angle between two angles
    /// </summary>
    /// <param name="angle">Target angle</param>
    /// <param name="base">Base angle</param>
    /// <returns>Closest angle</returns>
    public static double ClosestAngle(double angle, double @base)
    {
        var difference = Math.Abs(angle - @base);
        return difference > 180 ? 360 - difference : difference;
    }
}
