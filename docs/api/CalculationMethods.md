# Calculation Methods

AzanDotNet supports various calculation methods from major Islamic organizations worldwide.

## Available Methods

### Muslim World League
```csharp
var parameters = CalculationMethod.MuslimWorldLeague();
```
- Fajr Angle: 18°
- Isha Angle: 17°
- Used by: Muslim World League

### Egyptian General Authority
```csharp
var parameters = CalculationMethod.Egyptian();
```
- Fajr Angle: 19.5°
- Isha Angle: 17.5°
- Used by: Egyptian General Authority of Survey

### University of Islamic Sciences, Karachi
```csharp
var parameters = CalculationMethod.Karachi();
```
- Fajr Angle: 18°
- Isha Angle: 18°
- Used by: University of Islamic Sciences, Karachi

### Umm al-Qura University, Makkah
```csharp
var parameters = CalculationMethod.UmmAlQura();
```
- Fajr Angle: 18.5°
- Isha Interval: 90 minutes after Maghrib
- Used by: Umm al-Qura University, Makkah

### Islamic Society of North America (ISNA)
```csharp
var parameters = CalculationMethod.NorthAmerica();
```
- Fajr Angle: 15°
- Isha Angle: 15°
- Used by: Islamic Society of North America

### Dubai
```csharp
var parameters = CalculationMethod.Dubai();
```
- Fajr Angle: 18.2°
- Isha Angle: 18.2°
- Used by: Dubai Islamic Affairs

### Kuwait
```csharp
var parameters = CalculationMethod.Kuwait();
```
- Fajr Angle: 18°
- Isha Angle: 17.5°
- Used by: Kuwait Ministry of Awqaf

### Qatar
```csharp
var parameters = CalculationMethod.Qatar();
```
- Fajr Angle: 18°
- Isha Interval: 90 minutes after Maghrib
- Used by: Qatar Ministry of Islamic Affairs

### Singapore
```csharp
var parameters = CalculationMethod.Singapore();
```
- Fajr Angle: 20°
- Isha Angle: 18°
- Used by: Islamic Religious Council of Singapore

### Turkey (with Seasonal Adjustments)
```csharp
var parameters = CalculationMethod.TurkeyWithSeasonalAdjustments(DateTime.Today);
```
- Fajr Angle: 18°
- Isha Angle: 17°
- Includes seasonal adjustments for improved accuracy
- Used by: Diyanet İşleri Başkanlığı (Turkey)

## European Optimized Methods

### Best European Method
```csharp
var coordinates = new Coordinates(51.5074, -0.1278); // London
var parameters = CalculationMethod.BestEuropeanMethod(coordinates);
```
Returns the most accurate method for specific European cities based on real-world testing.

### Supported European Cities
- London, UK
- Stockholm, Sweden
- Copenhagen, Denmark
- Oslo, Norway
- Madrid, Spain
- Paris, France
- Berlin, Germany
- Rome, Italy
- And many more...

## Custom Parameters

You can also create custom calculation parameters:

```csharp
var customParameters = new CalculationParameters("Custom", 18.0, 17.0);
customParameters.Madhab = Madhab.Hanafi;
customParameters.HighLatitudeRule = HighLatitudeRule.SeventhOfTheNight;

// Apply custom adjustments
customParameters.MethodAdjustments.Fajr = 2;
customParameters.MethodAdjustments.Isha = -3;
```
