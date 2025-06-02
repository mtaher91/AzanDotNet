# AzanDotNet

**AzanDotNet** is a comprehensive and highly accurate Islamic prayer times calculation library designed specifically for .NET applications. Built with precision astronomical algorithms and extensive real-world testing, it provides reliable prayer time calculations for Muslims worldwide.

Whether you're developing Islamic mobile apps, mosque management systems, or any application requiring accurate prayer time calculations, AzanDotNet delivers the precision and flexibility you need with support for global locations and extensive customization options.

## Features

- 🕌 **Precision Prayer Times**: Calculate all five daily prayers (Fajr, Dhuhr, Asr, Maghrib, Isha) plus Sunrise with astronomical accuracy
- 🌍 **Multiple Calculation Methods**: Support for 15+ calculation methods from major Islamic organizations worldwide
- 🌐 **Global Coverage**: Works anywhere on Earth with automatic timezone and daylight saving time handling
- 📍 **Qibla Direction**: Accurate calculation of the direction to the Kaaba in Mecca from any location
- 🌙 **Sunnah Times**: Calculate recommended Islamic times including Tahajjud, Ishraq, and night portions
- 🏔️ **High Latitude Support**: Specialized algorithms for polar and extreme latitude regions
- 🇪🇺 **European Optimization**: City-specific optimized methods tested against real-world data
- 🕰️ **Flexible Adjustments**: Custom time adjustments and multiple madhab support (Hanafi, Shafi, etc.)
- ⚡ **High Performance**: Optimized astronomical calculations based on Jean Meeus algorithms
- 🎯 **Developer Friendly**: Clean, intuitive API with comprehensive documentation and examples
- 📱 **Cross-Platform**: Compatible with .NET 6+ across Windows, macOS, and Linux

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package AzanDotNet
```

Or via Package Manager Console:

```powershell
Install-Package AzanDotNet
```

## Quick Start

```csharp
using AzanDotNet;

// Set your location coordinates
var coordinates = new Coordinates(21.4225, 39.8262); // Mecca coordinates

// Choose a calculation method
var parameters = CalculationMethod.MuslimWorldLeague();

// Calculate prayer times for today
var prayerTimes = new PrayerTimes(coordinates, DateTime.Today, parameters);

// Display prayer times
Console.WriteLine($"Fajr: {prayerTimes.Fajr:HH:mm}");
Console.WriteLine($"Sunrise: {prayerTimes.Sunrise:HH:mm}");
Console.WriteLine($"Dhuhr: {prayerTimes.Dhuhr:HH:mm}");
Console.WriteLine($"Asr: {prayerTimes.Asr:HH:mm}");
Console.WriteLine($"Maghrib: {prayerTimes.Maghrib:HH:mm}");
Console.WriteLine($"Isha: {prayerTimes.Isha:HH:mm}");
```

## Calculation Methods

AzanDotNet supports various calculation methods:

- **Muslim World League** - `CalculationMethod.MuslimWorldLeague()`
- **Egyptian General Authority** - `CalculationMethod.Egyptian()`
- **University of Islamic Sciences, Karachi** - `CalculationMethod.Karachi()`
- **Umm al-Qura University, Makkah** - `CalculationMethod.UmmAlQura()`
- **Islamic Society of North America (ISNA)** - `CalculationMethod.NorthAmerica()`
- **Dubai** - `CalculationMethod.Dubai()`
- **Kuwait** - `CalculationMethod.Kuwait()`
- **Qatar** - `CalculationMethod.Qatar()`
- **Singapore** - `CalculationMethod.Singapore()`
- **Turkey (with seasonal adjustments)** - `CalculationMethod.TurkeyWithSeasonalAdjustments(date)`

## Advanced Usage

### Timezone Support

```csharp
var coordinates = new Coordinates(40.7128, -74.0060); // New York
var parameters = CalculationMethod.NorthAmerica();
var timeZone = "America/New_York";

var prayerTimesWithTz = new PrayerTimesWithTimeZone(
    coordinates, 
    DateTime.Today, 
    parameters, 
    timeZone
);
```

### Qibla Direction

```csharp
var coordinates = new Coordinates(40.7128, -74.0060); // New York
var qiblaDirection = Qibla.Direction(coordinates);
Console.WriteLine($"Qibla direction: {qiblaDirection:F2}°");
```

### Sunnah Times

```csharp
var sunnahTimes = new SunnahTimes(prayerTimes);
Console.WriteLine($"Last third of night: {sunnahTimes.LastThirdOfTheNight:HH:mm}");
Console.WriteLine($"Middle of night: {sunnahTimes.MiddleOfTheNight:HH:mm}");
```

### Custom Parameters

```csharp
var customParameters = new CalculationParameters("Custom", 18.0, 17.0);
customParameters.Madhab = Madhab.Hanafi; // For Asr calculation
customParameters.HighLatitudeRule = HighLatitudeRule.SeventhOfTheNight;

// Apply custom adjustments
customParameters.MethodAdjustments.Fajr = 2; // Add 2 minutes to Fajr
customParameters.MethodAdjustments.Isha = -3; // Subtract 3 minutes from Isha
```

## High Latitude Locations

For locations with extreme latitudes where normal calculation may not work:

```csharp
var parameters = CalculationMethod.MuslimWorldLeague();
parameters.HighLatitudeRule = HighLatitudeRule.MiddleOfTheNight;
// or
parameters.HighLatitudeRule = HighLatitudeRule.SeventhOfTheNight;
// or  
parameters.HighLatitudeRule = HighLatitudeRule.TwilightAngle;
```

## European Cities Optimization

For European cities, use the optimized method:

```csharp
var coordinates = new Coordinates(51.5074, -0.1278); // London
var parameters = CalculationMethod.BestEuropeanMethod(coordinates);
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Acknowledgments

- Based on astronomical algorithms from "Astronomical Algorithms" by Jean Meeus
- Inspired by the Adhan JavaScript library
- Prayer time calculation methods from various Islamic organizations worldwide
