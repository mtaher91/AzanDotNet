namespace AzanDotNet;

/// <summary>
/// Solar time calculations for a given date and location
/// </summary>
internal class SolarTime
{
    /// <summary>
    /// Observer coordinates
    /// </summary>
    public Coordinates Observer { get; }

    /// <summary>
    /// Solar coordinates for the date
    /// </summary>
    public SolarCoordinates Solar { get; }

    /// <summary>
    /// Solar coordinates for the previous day
    /// </summary>
    public SolarCoordinates PrevSolar { get; }

    /// <summary>
    /// Solar coordinates for the next day
    /// </summary>
    public SolarCoordinates NextSolar { get; }

    /// <summary>
    /// Approximate transit time
    /// </summary>
    public double ApproxTransit { get; }

    /// <summary>
    /// Transit time in hours
    /// </summary>
    public double Transit { get; }

    /// <summary>
    /// Sunrise time in hours
    /// </summary>
    public double Sunrise { get; }

    /// <summary>
    /// Sunset time in hours
    /// </summary>
    public double Sunset { get; }

    /// <summary>
    /// Initializes solar time calculations for the given date and coordinates
    /// </summary>
    /// <param name="date">Date for calculations</param>
    /// <param name="coordinates">Observer coordinates</param>
    public SolarTime(DateTime date, Coordinates coordinates)
    {
        var julianDay = Astronomical.JulianDay(date.Year, date.Month, date.Day, 0);

        Observer = coordinates;
        Solar = new SolarCoordinates(julianDay);
        PrevSolar = new SolarCoordinates(julianDay - 1);
        NextSolar = new SolarCoordinates(julianDay + 1);

        var m0 = Astronomical.ApproximateTransit(
            coordinates.Longitude,
            Solar.ApparentSiderealTime,
            Solar.RightAscension);

        var solarAltitude = -50.0 / 60.0;

        ApproxTransit = m0;

        Transit = Astronomical.CorrectedTransit(
            m0,
            coordinates.Longitude,
            Solar.ApparentSiderealTime,
            Solar.RightAscension,
            PrevSolar.RightAscension,
            NextSolar.RightAscension);

        Sunrise = Astronomical.CorrectedHourAngle(
            m0,
            solarAltitude,
            coordinates,
            false,
            Solar.ApparentSiderealTime,
            Solar.RightAscension,
            PrevSolar.RightAscension,
            NextSolar.RightAscension,
            Solar.Declination,
            PrevSolar.Declination,
            NextSolar.Declination);

        Sunset = Astronomical.CorrectedHourAngle(
            m0,
            solarAltitude,
            coordinates,
            true,
            Solar.ApparentSiderealTime,
            Solar.RightAscension,
            PrevSolar.RightAscension,
            NextSolar.RightAscension,
            Solar.Declination,
            PrevSolar.Declination,
            NextSolar.Declination);
    }

    /// <summary>
    /// Hour angle for the given solar altitude
    /// </summary>
    /// <param name="angle">Solar altitude angle in degrees</param>
    /// <param name="afterTransit">Whether this is after transit (sunset) or before (sunrise)</param>
    /// <returns>Hour angle in hours</returns>
    public double HourAngle(double angle, bool afterTransit)
    {
        return Astronomical.CorrectedHourAngle(
            ApproxTransit,
            angle,
            Observer,
            afterTransit,
            Solar.ApparentSiderealTime,
            Solar.RightAscension,
            PrevSolar.RightAscension,
            NextSolar.RightAscension,
            Solar.Declination,
            PrevSolar.Declination,
            NextSolar.Declination);
    }

    /// <summary>
    /// Afternoon hour angle for the given shadow length
    /// </summary>
    /// <param name="shadowLength">Shadow length multiplier</param>
    /// <returns>Hour angle in hours</returns>
    public double Afternoon(double shadowLength)
    {
        // Calculate the solar altitude angle for the given shadow length
        // Based on the JavaScript implementation
        var tangent = Math.Abs(Observer.Latitude - Solar.Declination);
        var inverse = shadowLength + Math.Tan(MathUtils.DegreesToRadians(tangent));
        var angle = MathUtils.RadiansToDegrees(Math.Atan(1.0 / inverse));
        return HourAngle(angle, true);
    }
}
