namespace AzanDotNet;

/// <summary>
/// Represents geographical coordinates (latitude and longitude)
/// </summary>
public class Coordinates
{
    /// <summary>
    /// Latitude in degrees
    /// </summary>
    public double Latitude { get; }
    
    /// <summary>
    /// Longitude in degrees
    /// </summary>
    public double Longitude { get; }

    /// <summary>
    /// Initializes a new instance of the Coordinates class
    /// </summary>
    /// <param name="latitude">Latitude in degrees (-90 to 90)</param>
    /// <param name="longitude">Longitude in degrees (-180 to 180)</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when latitude or longitude is out of valid range</exception>
    public Coordinates(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), 
                "Latitude must be between -90 and 90 degrees");
        }
        
        if (longitude < -180 || longitude > 180)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), 
                "Longitude must be between -180 and 180 degrees");
        }

        Latitude = latitude;
        Longitude = longitude;
    }

    /// <summary>
    /// Returns a string representation of the coordinates
    /// </summary>
    /// <returns>String in format "Latitude, Longitude"</returns>
    public override string ToString()
    {
        return $"{Latitude:F6}, {Longitude:F6}";
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current coordinates
    /// </summary>
    /// <param name="obj">The object to compare</param>
    /// <returns>True if the objects are equal, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Coordinates other)
            return false;

        return Math.Abs(Latitude - other.Latitude) < 1e-9 && 
               Math.Abs(Longitude - other.Longitude) < 1e-9;
    }

    /// <summary>
    /// Returns a hash code for the coordinates
    /// </summary>
    /// <returns>Hash code</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }
}
