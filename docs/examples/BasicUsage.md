# Basic Usage Examples

This document provides basic usage examples for the AzanDotNet library.

## Simple Prayer Times Calculation

```csharp
using AzanDotNet;

// Set coordinates for your location
var coordinates = new Coordinates(21.4225, 39.8262); // Mecca

// Choose a calculation method
var parameters = CalculationMethod.MuslimWorldLeague();

// Calculate prayer times for today
var prayerTimes = new PrayerTimes(coordinates, DateTime.Today, parameters);

// Display the prayer times
Console.WriteLine($"Fajr: {prayerTimes.Fajr:HH:mm}");
Console.WriteLine($"Sunrise: {prayerTimes.Sunrise:HH:mm}");
Console.WriteLine($"Dhuhr: {prayerTimes.Dhuhr:HH:mm}");
Console.WriteLine($"Asr: {prayerTimes.Asr:HH:mm}");
Console.WriteLine($"Maghrib: {prayerTimes.Maghrib:HH:mm}");
Console.WriteLine($"Isha: {prayerTimes.Isha:HH:mm}");
```

## Qibla Direction

```csharp
using AzanDotNet;

var coordinates = new Coordinates(40.7128, -74.0060); // New York
var qiblaDirection = Qibla.GetDirection(coordinates);
Console.WriteLine($"Qibla direction: {qiblaDirection:F2}°");
```

## Different Calculation Methods

```csharp
using AzanDotNet;

var coordinates = new Coordinates(40.7128, -74.0060); // New York

// Islamic Society of North America (ISNA)
var isnaParams = CalculationMethod.NorthAmerica();
var isnaTimes = new PrayerTimes(coordinates, DateTime.Today, isnaParams);

// Egyptian General Authority
var egyptianParams = CalculationMethod.Egyptian();
var egyptianTimes = new PrayerTimes(coordinates, DateTime.Today, egyptianParams);

// University of Islamic Sciences, Karachi
var karachiParams = CalculationMethod.Karachi();
var karachiTimes = new PrayerTimes(coordinates, DateTime.Today, karachiParams);
```

## Sunnah Times

```csharp
using AzanDotNet;

var coordinates = new Coordinates(21.4225, 39.8262);
var parameters = CalculationMethod.MuslimWorldLeague();
var prayerTimes = new PrayerTimes(coordinates, DateTime.Today, parameters);

// Calculate Sunnah times
var sunnahTimes = new SunnahTimes(prayerTimes);

Console.WriteLine($"Middle of Night: {sunnahTimes.MiddleOfTheNight:HH:mm}");
Console.WriteLine($"Last Third of Night: {sunnahTimes.LastThirdOfTheNight:HH:mm}");
```

## Custom Adjustments

```csharp
using AzanDotNet;

var parameters = CalculationMethod.MuslimWorldLeague();

// Add 2 minutes to Fajr
parameters.MethodAdjustments.Fajr = 2;

// Subtract 3 minutes from Isha
parameters.MethodAdjustments.Isha = -3;

// Use Hanafi madhab for Asr calculation
parameters.Madhab = Madhab.Hanafi;

var coordinates = new Coordinates(40.7128, -74.0060);
var prayerTimes = new PrayerTimes(coordinates, DateTime.Today, parameters);
```
