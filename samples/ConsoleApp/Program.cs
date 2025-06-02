using AzanDotNet;

Console.WriteLine("=== AzanDotNet Sample Application ===");
Console.WriteLine();

// Example 1: Basic Prayer Times Calculation
Console.WriteLine("1. Basic Prayer Times for Mecca:");
var meccaCoordinates = new Coordinates(21.4225, 39.8262);
var parameters = CalculationMethod.MuslimWorldLeague();
var prayerTimes = new PrayerTimes(meccaCoordinates, DateTime.Today, parameters);

Console.WriteLine($"   Fajr:    {prayerTimes.Fajr:HH:mm}");
Console.WriteLine($"   Sunrise: {prayerTimes.Sunrise:HH:mm}");
Console.WriteLine($"   Dhuhr:   {prayerTimes.Dhuhr:HH:mm}");
Console.WriteLine($"   Asr:     {prayerTimes.Asr:HH:mm}");
Console.WriteLine($"   Maghrib: {prayerTimes.Maghrib:HH:mm}");
Console.WriteLine($"   Isha:    {prayerTimes.Isha:HH:mm}");
Console.WriteLine();

// Example 2: Qibla Direction
Console.WriteLine("2. Qibla Direction from New York:");
var newYorkCoordinates = new Coordinates(40.7128, -74.0060);
var qiblaDirection = Qibla.GetDirection(newYorkCoordinates);
Console.WriteLine($"   Qibla Direction: {qiblaDirection:F2}°");
Console.WriteLine();

// Example 3: Sunnah Times
Console.WriteLine("3. Sunnah Times:");
var sunnahTimes = new SunnahTimes(prayerTimes);
Console.WriteLine($"   Middle of Night: {sunnahTimes.MiddleOfTheNight:HH:mm}");
Console.WriteLine($"   Last Third of Night: {sunnahTimes.LastThirdOfTheNight:HH:mm}");
Console.WriteLine();

// Example 4: Different Calculation Method
Console.WriteLine("4. Prayer Times using North America method:");
var naParameters = CalculationMethod.NorthAmerica();
var naPrayerTimes = new PrayerTimes(newYorkCoordinates, DateTime.Today, naParameters);
Console.WriteLine($"   Fajr: {naPrayerTimes.Fajr:HH:mm}");
Console.WriteLine($"   Isha: {naPrayerTimes.Isha:HH:mm}");
Console.WriteLine();

Console.WriteLine("=== Detailed Comparison Test ===");
Console.WriteLine();
ComparisonTest.RunComparisons();

Console.WriteLine();
Console.WriteLine("=== COMPREHENSIVE ACCURACY ANALYSIS ===");
Console.WriteLine();
AccuracyAnalysis.RunComprehensiveAnalysis();

Console.WriteLine();
Console.WriteLine("=== GLOBAL ACCURACY TEST - WORLDWIDE VALIDATION ===");
Console.WriteLine();
GlobalAccuracyTest.RunGlobalAccuracyTest();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
