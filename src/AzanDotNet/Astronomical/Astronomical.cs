namespace AzanDotNet;

/// <summary>
/// Astronomical calculations for prayer times
/// All calculations are based on "Astronomical Algorithms" by Jean Meeus
/// </summary>
internal static class Astronomical
{
    /// <summary>
    /// The geometric mean longitude of the sun in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Mean solar longitude in degrees</returns>
    public static double MeanSolarLongitude(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 163
        var term1 = 280.4664567;
        var term2 = 36000.76983 * T;
        var term3 = 0.0003032 * Math.Pow(T, 2);
        var L0 = term1 + term2 + term3;
        return MathUtils.UnwindAngle(L0);
    }

    /// <summary>
    /// The geometric mean longitude of the moon in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Mean lunar longitude in degrees</returns>
    public static double MeanLunarLongitude(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 144
        var term1 = 218.3165;
        var term2 = 481267.8813 * T;
        var Lp = term1 + term2;
        return MathUtils.UnwindAngle(Lp);
    }

    /// <summary>
    /// The ascending lunar node longitude
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Ascending lunar node longitude in degrees</returns>
    public static double AscendingLunarNodeLongitude(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 144
        var term1 = 125.04452;
        var term2 = 1934.136261 * T;
        var term3 = 0.0020708 * Math.Pow(T, 2);
        var term4 = Math.Pow(T, 3) / 450000;
        var Omega = term1 - term2 + term3 + term4;
        return MathUtils.UnwindAngle(Omega);
    }

    /// <summary>
    /// The mean anomaly of the sun
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Mean solar anomaly in degrees</returns>
    public static double MeanSolarAnomaly(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 163
        var term1 = 357.52911;
        var term2 = 35999.05029 * T;
        var term3 = 0.0001537 * Math.Pow(T, 2);
        var M = term1 + term2 - term3;
        return MathUtils.UnwindAngle(M);
    }

    /// <summary>
    /// The Sun's equation of the center in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <param name="meanAnomaly">Mean anomaly in degrees</param>
    /// <returns>Solar equation of the center in degrees</returns>
    public static double SolarEquationOfTheCenter(double julianCentury, double meanAnomaly)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 164
        var Mrad = MathUtils.DegreesToRadians(meanAnomaly);
        var term1 = (1.914602 - 0.004817 * T - 0.000014 * Math.Pow(T, 2)) * Math.Sin(Mrad);
        var term2 = (0.019993 - 0.000101 * T) * Math.Sin(2 * Mrad);
        var term3 = 0.000289 * Math.Sin(3 * Mrad);
        return term1 + term2 + term3;
    }

    /// <summary>
    /// The apparent longitude of the Sun, referred to the true equinox of the date
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <param name="meanLongitude">Mean longitude in degrees</param>
    /// <returns>Apparent solar longitude in degrees</returns>
    public static double ApparentSolarLongitude(double julianCentury, double meanLongitude)
    {
        var T = julianCentury;
        var L0 = meanLongitude;
        // Equation from Astronomical Algorithms page 164
        var longitude = L0 + SolarEquationOfTheCenter(T, MeanSolarAnomaly(T));
        var Omega = 125.04 - 1934.136 * T;
        var Lambda = longitude - 0.00569 - 0.00478 * Math.Sin(MathUtils.DegreesToRadians(Omega));
        return MathUtils.UnwindAngle(Lambda);
    }

    /// <summary>
    /// The mean obliquity of the ecliptic in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Mean obliquity of the ecliptic in degrees</returns>
    public static double MeanObliquityOfTheEcliptic(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 147
        var term1 = 23.439291;
        var term2 = 0.013004167 * T;
        var term3 = 0.0000001639 * Math.Pow(T, 2);
        var term4 = 0.0000005036 * Math.Pow(T, 3);
        return term1 - term2 - term3 + term4;
    }

    /// <summary>
    /// The mean obliquity of the ecliptic, corrected for calculating the apparent position of the sun, in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <param name="meanObliquityOfTheEcliptic">Mean obliquity of the ecliptic</param>
    /// <returns>Apparent obliquity of the ecliptic in degrees</returns>
    public static double ApparentObliquityOfTheEcliptic(double julianCentury, double meanObliquityOfTheEcliptic)
    {
        var T = julianCentury;
        var Epsilon0 = meanObliquityOfTheEcliptic;
        // Equation from Astronomical Algorithms page 165
        var O = 125.04 - 1934.136 * T;
        return Epsilon0 + 0.00256 * Math.Cos(MathUtils.DegreesToRadians(O));
    }

    /// <summary>
    /// Mean sidereal time, the hour angle of the vernal equinox, in degrees
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <returns>Mean sidereal time in degrees</returns>
    public static double MeanSiderealTime(double julianCentury)
    {
        var T = julianCentury;
        // Equation from Astronomical Algorithms page 165
        var JD = T * 36525 + 2451545.0;
        var term1 = 280.46061837;
        var term2 = 360.98564736629 * (JD - 2451545);
        var term3 = 0.000387933 * Math.Pow(T, 2);
        var term4 = Math.Pow(T, 3) / 38710000;
        var Theta = term1 + term2 + term3 - term4;
        return MathUtils.UnwindAngle(Theta);
    }

    /// <summary>
    /// Nutation in longitude
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <param name="solarLongitude">Solar longitude</param>
    /// <param name="lunarLongitude">Lunar longitude</param>
    /// <param name="ascendingNode">Ascending node</param>
    /// <returns>Nutation in longitude</returns>
    public static double NutationInLongitude(double julianCentury, double solarLongitude, 
        double lunarLongitude, double ascendingNode)
    {
        var L0 = solarLongitude;
        var Lp = lunarLongitude;
        var Omega = ascendingNode;
        // Equation from Astronomical Algorithms page 144
        var term1 = (-17.2 / 3600) * Math.Sin(MathUtils.DegreesToRadians(Omega));
        var term2 = (1.32 / 3600) * Math.Sin(MathUtils.DegreesToRadians(2 * L0));
        var term3 = (0.23 / 3600) * Math.Sin(MathUtils.DegreesToRadians(2 * Lp));
        var term4 = (0.21 / 3600) * Math.Sin(MathUtils.DegreesToRadians(2 * Omega));
        return term1 - term2 - term3 + term4;
    }

    /// <summary>
    /// Nutation in obliquity
    /// </summary>
    /// <param name="julianCentury">Julian century</param>
    /// <param name="solarLongitude">Solar longitude</param>
    /// <param name="lunarLongitude">Lunar longitude</param>
    /// <param name="ascendingNode">Ascending node</param>
    /// <returns>Nutation in obliquity</returns>
    public static double NutationInObliquity(double julianCentury, double solarLongitude, 
        double lunarLongitude, double ascendingNode)
    {
        var L0 = solarLongitude;
        var Lp = lunarLongitude;
        var Omega = ascendingNode;
        // Equation from Astronomical Algorithms page 144
        var term1 = (9.2 / 3600) * Math.Cos(MathUtils.DegreesToRadians(Omega));
        var term2 = (0.57 / 3600) * Math.Cos(MathUtils.DegreesToRadians(2 * L0));
        var term3 = (0.10 / 3600) * Math.Cos(MathUtils.DegreesToRadians(2 * Lp));
        var term4 = (0.09 / 3600) * Math.Cos(MathUtils.DegreesToRadians(2 * Omega));
        return term1 + term2 + term3 - term4;
    }

    /// <summary>
    /// Altitude of celestial body
    /// </summary>
    /// <param name="observerLatitude">Observer latitude in degrees</param>
    /// <param name="declination">Declination in degrees</param>
    /// <param name="localHourAngle">Local hour angle in degrees</param>
    /// <returns>Altitude in degrees</returns>
    public static double AltitudeOfCelestialBody(double observerLatitude, double declination, double localHourAngle)
    {
        var Phi = observerLatitude;
        var delta = declination;
        var H = localHourAngle;
        // Equation from Astronomical Algorithms page 93
        var term1 = Math.Sin(MathUtils.DegreesToRadians(Phi)) * Math.Sin(MathUtils.DegreesToRadians(delta));
        var term2 = Math.Cos(MathUtils.DegreesToRadians(Phi)) * 
                   Math.Cos(MathUtils.DegreesToRadians(delta)) * 
                   Math.Cos(MathUtils.DegreesToRadians(H));
        return MathUtils.RadiansToDegrees(Math.Asin(term1 + term2));
    }

    /// <summary>
    /// Approximate transit
    /// </summary>
    /// <param name="longitude">Longitude in degrees</param>
    /// <param name="siderealTime">Sidereal time in degrees</param>
    /// <param name="rightAscension">Right ascension in degrees</param>
    /// <returns>Approximate transit</returns>
    public static double ApproximateTransit(double longitude, double siderealTime, double rightAscension)
    {
        var L = longitude;
        var Theta0 = siderealTime;
        var a2 = rightAscension;
        // Equation from page Astronomical Algorithms 102
        var Lw = L * -1;
        return MathUtils.NormalizeToScale((a2 + Lw - Theta0) / 360, 1);
    }

    /// <summary>
    /// The time at which the sun is at its highest point in the sky (in universal time)
    /// </summary>
    /// <param name="approximateTransit">Approximate transit</param>
    /// <param name="longitude">Longitude in degrees</param>
    /// <param name="siderealTime">Sidereal time in degrees</param>
    /// <param name="rightAscension">Right ascension in degrees</param>
    /// <param name="previousRightAscension">Previous right ascension in degrees</param>
    /// <param name="nextRightAscension">Next right ascension in degrees</param>
    /// <returns>Corrected transit in hours</returns>
    public static double CorrectedTransit(double approximateTransit, double longitude, double siderealTime,
        double rightAscension, double previousRightAscension, double nextRightAscension)
    {
        var m0 = approximateTransit;
        var L = longitude;
        var Theta0 = siderealTime;
        var a2 = rightAscension;
        var a1 = previousRightAscension;
        var a3 = nextRightAscension;
        // Equation from page Astronomical Algorithms 102
        var Lw = L * -1;
        var Theta = MathUtils.UnwindAngle(Theta0 + 360.985647 * m0);
        var a = MathUtils.UnwindAngle(InterpolateAngles(a2, a1, a3, m0));
        var H = MathUtils.QuadrantShiftAngle(Theta - Lw - a);
        var dm = H / -360;
        return (m0 + dm) * 24;
    }

    /// <summary>
    /// Corrected hour angle
    /// </summary>
    public static double CorrectedHourAngle(double approximateTransit, double angle, Coordinates coordinates,
        bool afterTransit, double siderealTime, double rightAscension, double previousRightAscension,
        double nextRightAscension, double declination, double previousDeclination, double nextDeclination)
    {
        var m0 = approximateTransit;
        var h0 = angle;
        var Theta0 = siderealTime;
        var a2 = rightAscension;
        var a1 = previousRightAscension;
        var a3 = nextRightAscension;
        var d2 = declination;
        var d1 = previousDeclination;
        var d3 = nextDeclination;

        // Equation from page Astronomical Algorithms 102
        var Lw = coordinates.Longitude * -1;
        var term1 = Math.Sin(MathUtils.DegreesToRadians(h0)) -
                   Math.Sin(MathUtils.DegreesToRadians(coordinates.Latitude)) *
                   Math.Sin(MathUtils.DegreesToRadians(d2));
        var term2 = Math.Cos(MathUtils.DegreesToRadians(coordinates.Latitude)) *
                   Math.Cos(MathUtils.DegreesToRadians(d2));

        // Handle edge cases for acos domain [-1, 1]
        var cosValue = term1 / term2;
        if (cosValue > 1.0)
        {
            cosValue = 1.0;
        }
        else if (cosValue < -1.0)
        {
            cosValue = -1.0;
        }

        var H0 = MathUtils.RadiansToDegrees(Math.Acos(cosValue));
        var m = afterTransit ? m0 + H0 / 360 : m0 - H0 / 360;
        var Theta = MathUtils.UnwindAngle(Theta0 + 360.985647 * m);
        var a = MathUtils.UnwindAngle(InterpolateAngles(a2, a1, a3, m));
        var delta = Interpolate(d2, d1, d3, m);
        var H = Theta - Lw - a;
        var h = AltitudeOfCelestialBody(coordinates.Latitude, delta, H);
        var term3 = h - h0;
        var term4 = 360 * Math.Cos(MathUtils.DegreesToRadians(delta)) *
                   Math.Cos(MathUtils.DegreesToRadians(coordinates.Latitude)) *
                   Math.Sin(MathUtils.DegreesToRadians(H));
        var dm = term3 / term4;
        return (m + dm) * 24;
    }

    /// <summary>
    /// Interpolates a value given equidistant interpolation points
    /// </summary>
    /// <param name="y2">Current value</param>
    /// <param name="y1">Previous value</param>
    /// <param name="y3">Next value</param>
    /// <param name="n">Interpolation factor</param>
    /// <returns>Interpolated value</returns>
    public static double Interpolate(double y2, double y1, double y3, double n)
    {
        // Equation from Astronomical Algorithms page 24
        var a = y2 - y1;
        var b = y3 - y2;
        var c = b - a;
        return y2 + n / 2 * (a + b + n * c);
    }

    /// <summary>
    /// Interpolates angles, accounting for angle unwinding
    /// </summary>
    /// <param name="y2">Current angle</param>
    /// <param name="y1">Previous angle</param>
    /// <param name="y3">Next angle</param>
    /// <param name="n">Interpolation factor</param>
    /// <returns>Interpolated angle</returns>
    public static double InterpolateAngles(double y2, double y1, double y3, double n)
    {
        // Equation from Astronomical Algorithms page 24
        var a = MathUtils.UnwindAngle(y2 - y1);
        var b = MathUtils.UnwindAngle(y3 - y2);
        var c = b - a;
        return y2 + n / 2 * (a + b + n * c);
    }

    /// <summary>
    /// The Julian Day for the given Gregorian date components
    /// </summary>
    /// <param name="year">Year</param>
    /// <param name="month">Month (1-12)</param>
    /// <param name="day">Day of month</param>
    /// <param name="hours">Hours (default 0)</param>
    /// <returns>Julian Day</returns>
    public static double JulianDay(int year, int month, int day, double hours = 0)
    {
        // Equation from Astronomical Algorithms page 60
        var Y = Math.Truncate((double)(month > 2 ? year : year - 1));
        var M = Math.Truncate((double)(month > 2 ? month : month + 12));
        var D = day + hours / 24;

        var A = Math.Truncate(Y / 100);
        var B = Math.Truncate(2 - A + Math.Truncate(A / 4));

        var i0 = Math.Truncate(365.25 * (Y + 4716));
        var i1 = Math.Truncate(30.6001 * (M + 1));

        return i0 + i1 + D + B - 1524.5;
    }

    /// <summary>
    /// Julian century from the epoch
    /// </summary>
    /// <param name="julianDay">Julian Day</param>
    /// <returns>Julian century</returns>
    public static double JulianCentury(double julianDay)
    {
        // Equation from Astronomical Algorithms page 163
        return (julianDay - 2451545.0) / 36525;
    }

    /// <summary>
    /// Whether or not a year is a leap year (has 366 days)
    /// </summary>
    /// <param name="year">Year</param>
    /// <returns>True if leap year, false otherwise</returns>
    public static bool IsLeapYear(int year)
    {
        if (year % 4 != 0)
            return false;

        if (year % 100 == 0 && year % 400 != 0)
            return false;

        return true;
    }

    /// <summary>
    /// Calculates season-adjusted morning twilight time
    /// </summary>
    /// <param name="latitude">Observer latitude</param>
    /// <param name="dayOfYear">Day of year (1-365/366)</param>
    /// <param name="year">Year</param>
    /// <param name="sunrise">Sunrise time</param>
    /// <returns>Season-adjusted morning twilight time</returns>
    public static DateTime SeasonAdjustedMorningTwilight(double latitude, int dayOfYear, int year, DateTime sunrise)
    {
        var a = 75 + (28.65 / 55.0) * Math.Abs(latitude);
        var b = 75 + (19.44 / 55.0) * Math.Abs(latitude);
        var c = 75 + (32.74 / 55.0) * Math.Abs(latitude);
        var d = 75 + (48.1 / 55.0) * Math.Abs(latitude);

        var dyy = DaysSinceSolstice(dayOfYear, year, latitude);
        double adjustment;

        if (dyy < 91)
        {
            adjustment = a + ((b - a) / 91.0) * dyy;
        }
        else if (dyy < 137)
        {
            adjustment = b + ((c - b) / 46.0) * (dyy - 91);
        }
        else if (dyy < 183)
        {
            adjustment = c + ((d - c) / 46.0) * (dyy - 137);
        }
        else if (dyy < 229)
        {
            adjustment = d + ((c - d) / 46.0) * (dyy - 183);
        }
        else if (dyy < 275)
        {
            adjustment = c + ((b - c) / 46.0) * (dyy - 229);
        }
        else
        {
            adjustment = b + ((a - b) / 91.0) * (dyy - 275);
        }

        return sunrise.AddSeconds(Math.Round(adjustment * -60.0));
    }

    /// <summary>
    /// Calculates season-adjusted evening twilight time
    /// </summary>
    /// <param name="latitude">Observer latitude</param>
    /// <param name="dayOfYear">Day of year (1-365/366)</param>
    /// <param name="year">Year</param>
    /// <param name="sunset">Sunset time</param>
    /// <param name="shafaq">Shafaq type</param>
    /// <returns>Season-adjusted evening twilight time</returns>
    public static DateTime SeasonAdjustedEveningTwilight(double latitude, int dayOfYear, int year, DateTime sunset, Shafaq shafaq = Shafaq.General)
    {
        double a, b, c, d;

        if (shafaq == Shafaq.Ahmer)
        {
            a = 62 + (17.4 / 55.0) * Math.Abs(latitude);
            b = 62 - (7.16 / 55.0) * Math.Abs(latitude);
            c = 62 + (5.12 / 55.0) * Math.Abs(latitude);
            d = 62 + (19.44 / 55.0) * Math.Abs(latitude);
        }
        else if (shafaq == Shafaq.Abyad)
        {
            a = 75 + (25.6 / 55.0) * Math.Abs(latitude);
            b = 75 + (7.16 / 55.0) * Math.Abs(latitude);
            c = 75 + (36.84 / 55.0) * Math.Abs(latitude);
            d = 75 + (81.84 / 55.0) * Math.Abs(latitude);
        }
        else // General
        {
            a = 75 + (25.6 / 55.0) * Math.Abs(latitude);
            b = 75 + (2.05 / 55.0) * Math.Abs(latitude);
            c = 75 - (9.21 / 55.0) * Math.Abs(latitude);
            d = 75 + (6.14 / 55.0) * Math.Abs(latitude);
        }

        var dyy = DaysSinceSolstice(dayOfYear, year, latitude);
        double adjustment;

        if (dyy < 91)
        {
            adjustment = a + ((b - a) / 91.0) * dyy;
        }
        else if (dyy < 137)
        {
            adjustment = b + ((c - b) / 46.0) * (dyy - 91);
        }
        else if (dyy < 183)
        {
            adjustment = c + ((d - c) / 46.0) * (dyy - 137);
        }
        else if (dyy < 229)
        {
            adjustment = d + ((c - d) / 46.0) * (dyy - 183);
        }
        else if (dyy < 275)
        {
            adjustment = c + ((b - c) / 46.0) * (dyy - 229);
        }
        else
        {
            adjustment = b + ((a - b) / 91.0) * (dyy - 275);
        }

        return sunset.AddSeconds(Math.Round(adjustment * 60.0));
    }

    /// <summary>
    /// Calculates days since solstice
    /// </summary>
    /// <param name="dayOfYear">Day of year</param>
    /// <param name="year">Year</param>
    /// <param name="latitude">Latitude</param>
    /// <returns>Days since solstice</returns>
    public static int DaysSinceSolstice(int dayOfYear, int year, double latitude)
    {
        var northernOffset = 10;
        var southernOffset = IsLeapYear(year) ? 173 : 172;
        var daysInYear = IsLeapYear(year) ? 366 : 365;

        int daysSinceSolstice;

        if (latitude >= 0)
        {
            daysSinceSolstice = dayOfYear + northernOffset;
            if (daysSinceSolstice >= daysInYear)
            {
                daysSinceSolstice = daysSinceSolstice - daysInYear;
            }
        }
        else
        {
            daysSinceSolstice = dayOfYear - southernOffset;
            if (daysSinceSolstice < 0)
            {
                daysSinceSolstice = daysSinceSolstice + daysInYear;
            }
        }

        return daysSinceSolstice;
    }
}
