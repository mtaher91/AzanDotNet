namespace AzanDotNet;

/// <summary>
/// Utility methods for timezone handling and coordinate-based timezone detection
/// </summary>
public static class TimezoneUtils
{
    /// <summary>
    /// Gets the approximate timezone for given coordinates
    /// This is a simplified implementation based on longitude
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <returns>Approximate TimeZoneInfo</returns>
    public static TimeZoneInfo GetTimezoneFromCoordinates(Coordinates coordinates)
    {
        // Simple timezone estimation based on longitude
        // Each 15 degrees of longitude represents approximately 1 hour
        var offsetHours = Math.Round(coordinates.Longitude / 15.0);
        
        // Clamp to valid timezone range (-12 to +14)
        offsetHours = Math.Max(-12, Math.Min(14, offsetHours));
        
        var offset = TimeSpan.FromHours(offsetHours);
        
        // Try to find a system timezone that matches this offset
        var systemTimezone = FindSystemTimezone(offset, coordinates);
        if (systemTimezone != null)
        {
            return systemTimezone;
        }
        
        // Fallback to creating a custom timezone
        var timezoneId = $"UTC{(offsetHours >= 0 ? "+" : "")}{offsetHours:F0}";
        return TimeZoneInfo.CreateCustomTimeZone(timezoneId, offset, timezoneId, timezoneId);
    }

    /// <summary>
    /// Gets timezone for well-known cities
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <returns>TimeZoneInfo for known cities, null otherwise</returns>
    public static TimeZoneInfo? GetKnownCityTimezone(Coordinates coordinates)
    {
        // Define well-known cities with their timezones
        var knownCities = new[]
        {
            // Middle East
            (21.4225, 39.8262, "Arab Standard Time"), // Mecca
            (24.7136, 46.6753, "Arab Standard Time"), // Riyadh
            (25.2048, 55.2708, "Arabian Standard Time"), // Dubai
            (29.3117, 47.4818, "Arab Standard Time"), // Kuwait City
            (25.3548, 51.1839, "Arab Standard Time"), // Doha
            
            // North America
            (40.7128, -74.0060, "Eastern Standard Time"), // New York
            (34.0522, -118.2437, "Pacific Standard Time"), // Los Angeles
            (41.8781, -87.6298, "Central Standard Time"), // Chicago
            (29.7604, -95.3698, "Central Standard Time"), // Houston
            
            // Europe
            (51.5074, -0.1278, "GMT Standard Time"), // London
            (48.8566, 2.3522, "Romance Standard Time"), // Paris
            (52.5200, 13.4050, "W. Europe Standard Time"), // Berlin
            (41.9028, 12.4964, "W. Europe Standard Time"), // Rome
            (40.4168, -3.7038, "Romance Standard Time"), // Madrid
            
            // Asia
            (35.6762, 139.6503, "Tokyo Standard Time"), // Tokyo
            (39.9042, 116.4074, "China Standard Time"), // Beijing
            (1.3521, 103.8198, "Singapore Standard Time"), // Singapore
            (19.0760, 72.8777, "India Standard Time"), // Mumbai
            
            // Others
            (-33.8688, 151.2093, "AUS Eastern Standard Time"), // Sydney
            (30.0444, 31.2357, "Egypt Standard Time"), // Cairo
            (41.0082, 28.9784, "Turkey Standard Time"), // Istanbul
        };

        const double tolerance = 0.5; // 0.5 degree tolerance (~55km)
        
        foreach (var (lat, lon, timezoneId) in knownCities)
        {
            var distance = Math.Sqrt(Math.Pow(coordinates.Latitude - lat, 2) + 
                                   Math.Pow(coordinates.Longitude - lon, 2));
            
            if (distance <= tolerance)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
                }
                catch
                {
                    // Timezone not found on this system, continue
                }
            }
        }
        
        return null;
    }

    /// <summary>
    /// Finds a system timezone that matches the given offset and coordinates
    /// </summary>
    /// <param name="offset">Target UTC offset</param>
    /// <param name="coordinates">Location coordinates</param>
    /// <returns>Matching TimeZoneInfo or null</returns>
    private static TimeZoneInfo? FindSystemTimezone(TimeSpan offset, Coordinates coordinates)
    {
        // First try to find a timezone for known cities
        var knownTimezone = GetKnownCityTimezone(coordinates);
        if (knownTimezone != null)
        {
            return knownTimezone;
        }
        
        // Find system timezones that match the offset
        var matchingTimezones = TimeZoneInfo.GetSystemTimeZones()
            .Where(tz => tz.BaseUtcOffset == offset)
            .ToList();
        
        if (matchingTimezones.Count == 0)
        {
            return null;
        }
        
        // Prefer timezones based on geographic location
        if (coordinates.Longitude >= -180 && coordinates.Longitude < -60) // Americas
        {
            var americanTimezone = matchingTimezones.FirstOrDefault(tz => 
                tz.Id.Contains("America") || tz.Id.Contains("US") || tz.Id.Contains("Canada"));
            if (americanTimezone != null) return americanTimezone;
        }
        else if (coordinates.Longitude >= -60 && coordinates.Longitude < 20) // Atlantic/Europe/Africa
        {
            var europeanTimezone = matchingTimezones.FirstOrDefault(tz => 
                tz.Id.Contains("Europe") || tz.Id.Contains("GMT") || tz.Id.Contains("Atlantic"));
            if (europeanTimezone != null) return europeanTimezone;
        }
        else if (coordinates.Longitude >= 20 && coordinates.Longitude < 180) // Asia/Pacific
        {
            var asianTimezone = matchingTimezones.FirstOrDefault(tz => 
                tz.Id.Contains("Asia") || tz.Id.Contains("Pacific") || tz.Id.Contains("Australia"));
            if (asianTimezone != null) return asianTimezone;
        }
        
        // Return the first matching timezone
        return matchingTimezones.First();
    }

    /// <summary>
    /// Creates a PrayerTimes instance with automatic timezone detection
    /// </summary>
    /// <param name="coordinates">Location coordinates</param>
    /// <param name="date">Date for prayer times</param>
    /// <param name="calculationParameters">Calculation parameters</param>
    /// <returns>PrayerTimes with local timezone</returns>
    public static PrayerTimes CreateWithAutoTimezone(Coordinates coordinates, DateTime date, CalculationParameters calculationParameters)
    {
        var timezone = GetTimezoneFromCoordinates(coordinates);
        return new PrayerTimes(coordinates, date, calculationParameters, timezone);
    }
}
