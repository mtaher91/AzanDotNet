using AzanDotNet;

public static class ComparisonTest
{
    public static void RunComparisons()
    {
        var today = DateTime.Today;
        Console.WriteLine($"=== Prayer Times Comparison for {today:yyyy-MM-dd} ===");
        Console.WriteLine();

        // Test multiple locations with different calculation methods
        TestLocation("Mecca, Saudi Arabia", new Coordinates(21.4225, 39.8262), 
                    CalculationMethod.UmmAlQura(), "Umm al-Qura");
        
        TestLocation("New York, USA", new Coordinates(40.7128, -74.0060), 
                    CalculationMethod.NorthAmerica(), "ISNA");
        
        TestLocation("London, UK", new Coordinates(51.5074, -0.1278), 
                    CalculationMethod.MuslimWorldLeague(), "MWL");
        
        TestLocation("Cairo, Egypt", new Coordinates(30.0444, 31.2357), 
                    CalculationMethod.Egyptian(), "Egyptian Authority");
        
        TestLocation("Istanbul, Turkey", new Coordinates(41.0082, 28.9784), 
                    CalculationMethod.TurkeyWithSeasonalAdjustments(today), "Turkey (Seasonal)");
        
        TestLocation("Karachi, Pakistan", new Coordinates(24.8607, 67.0011), 
                    CalculationMethod.Karachi(), "Karachi University");
        
        TestLocation("Dubai, UAE", new Coordinates(25.2048, 55.2708), 
                    CalculationMethod.Dubai(), "Dubai");
        
        TestLocation("Singapore", new Coordinates(1.3521, 103.8198), 
                    CalculationMethod.Singapore(), "Singapore");

        // Test Qibla directions
        Console.WriteLine("=== Qibla Direction Comparisons ===");
        TestQibla("New York, USA", new Coordinates(40.7128, -74.0060));
        TestQibla("London, UK", new Coordinates(51.5074, -0.1278));
        TestQibla("Sydney, Australia", new Coordinates(-33.8688, 151.2093));
        TestQibla("Tokyo, Japan", new Coordinates(35.6762, 139.6503));
        Console.WriteLine();
    }

    private static void TestLocation(string cityName, Coordinates coordinates,
                                   CalculationParameters parameters, string methodName)
    {
        // Test with automatic timezone detection
        var prayerTimesLocal = TimezoneUtils.CreateWithAutoTimezone(coordinates, DateTime.Today, parameters);

        // Also test UTC for comparison
        var prayerTimesUtc = new PrayerTimes(coordinates, DateTime.Today, parameters);

        Console.WriteLine($"{cityName} ({methodName}):");
        Console.WriteLine($"  LOCAL TIME (Auto-detected timezone: {prayerTimesLocal.TimeZone?.DisplayName ?? "UTC"}):");
        Console.WriteLine($"    Fajr:    {prayerTimesLocal.Fajr:HH:mm}");
        Console.WriteLine($"    Sunrise: {prayerTimesLocal.Sunrise:HH:mm}");
        Console.WriteLine($"    Dhuhr:   {prayerTimesLocal.Dhuhr:HH:mm}");
        Console.WriteLine($"    Asr:     {prayerTimesLocal.Asr:HH:mm}");
        Console.WriteLine($"    Maghrib: {prayerTimesLocal.Maghrib:HH:mm}");
        Console.WriteLine($"    Isha:    {prayerTimesLocal.Isha:HH:mm}");
        Console.WriteLine($"  UTC TIME:");
        Console.WriteLine($"    Fajr:    {prayerTimesUtc.Fajr:HH:mm}");
        Console.WriteLine($"    Dhuhr:   {prayerTimesUtc.Dhuhr:HH:mm}");
        Console.WriteLine($"    Maghrib: {prayerTimesUtc.Maghrib:HH:mm}");
        Console.WriteLine();
    }

    private static void TestQibla(string cityName, Coordinates coordinates)
    {
        var qiblaDirection = Qibla.GetDirection(coordinates);
        Console.WriteLine($"{cityName}: {qiblaDirection:F2}°");
    }
}
