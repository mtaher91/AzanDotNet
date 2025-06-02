namespace AzanDotNet;

/// <summary>
/// Solar coordinates for a given Julian Day
/// </summary>
internal class SolarCoordinates
{
    /// <summary>
    /// Declination of the sun in degrees
    /// </summary>
    public double Declination { get; }

    /// <summary>
    /// Right ascension of the sun in degrees
    /// </summary>
    public double RightAscension { get; }

    /// <summary>
    /// Apparent sidereal time in degrees
    /// </summary>
    public double ApparentSiderealTime { get; }

    /// <summary>
    /// Initializes solar coordinates for the given Julian Day
    /// </summary>
    /// <param name="julianDay">Julian Day</param>
    public SolarCoordinates(double julianDay)
    {
        var T = Astronomical.JulianCentury(julianDay);
        var L0 = Astronomical.MeanSolarLongitude(T);
        var Lp = Astronomical.MeanLunarLongitude(T);
        var Omega = Astronomical.AscendingLunarNodeLongitude(T);
        var Lambda = MathUtils.DegreesToRadians(Astronomical.ApparentSolarLongitude(T, L0));
        var Theta0 = Astronomical.MeanSiderealTime(T);
        var dPsi = Astronomical.NutationInLongitude(T, L0, Lp, Omega);
        var dEpsilon = Astronomical.NutationInObliquity(T, L0, Lp, Omega);
        var Epsilon0 = Astronomical.MeanObliquityOfTheEcliptic(T);
        var EpsilonApparent = MathUtils.DegreesToRadians(
            Astronomical.ApparentObliquityOfTheEcliptic(T, Epsilon0));

        // Equation from Astronomical Algorithms page 165
        Declination = MathUtils.RadiansToDegrees(Math.Asin(
            Math.Sin(EpsilonApparent) * Math.Sin(Lambda)));

        RightAscension = MathUtils.UnwindAngle(MathUtils.RadiansToDegrees(
            Math.Atan2(Math.Cos(EpsilonApparent) * Math.Sin(Lambda), Math.Cos(Lambda))));

        ApparentSiderealTime = Theta0 + dPsi * Math.Cos(MathUtils.DegreesToRadians(Epsilon0 + dEpsilon));
    }
}
