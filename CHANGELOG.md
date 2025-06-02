# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-02-06

### Added
- Initial release of AzanDotNet library
- High precision Islamic prayer times calculation
- Support for 15+ calculation methods from major Islamic organizations
- Automatic timezone and daylight saving time handling
- Qibla direction calculation
- Sunnah times computation (Tahajjud, Ishraq, night portions)
- Specialized algorithms for high-latitude and polar regions
- European cities optimization with real-world tested methods
- Support for multiple madhabs (Hanafi, Shafi, Maliki, Hanbali)
- Custom time adjustments and flexible parameters
- Cross-platform compatibility (.NET 6+)
- Comprehensive documentation and examples

### Features
- **Core Classes**:
  - `PrayerTimes` - Main prayer times calculation
  - `PrayerTimesWithTimeZone` - Timezone-aware prayer times
  - `SunnahTimes` - Recommended Islamic times
  - `Qibla` - Direction to Mecca calculation

- **Calculation Methods**:
  - Muslim World League
  - Egyptian General Authority
  - University of Islamic Sciences, Karachi
  - Umm al-Qura University, Makkah
  - Islamic Society of North America (ISNA)
  - Dubai, Kuwait, Qatar, Singapore
  - Turkey with seasonal adjustments
  - European optimized methods

- **Models and Enums**:
  - `Coordinates` - Geographic coordinates
  - `Prayer` - Prayer types enumeration
  - `Madhab` - Islamic jurisprudence schools
  - `HighLatitudeRule` - Rules for extreme latitudes
  - `Shafaq` - Twilight types for Isha calculation

- **Utilities**:
  - Astronomical calculations based on Jean Meeus algorithms
  - Mathematical utilities for precise calculations
  - Date and time utilities
  - Timezone helper functions

### Technical Details
- Built with .NET 6.0
- Uses precision astronomical algorithms
- Optimized for performance and accuracy
- Comprehensive unit test coverage
- Clean, intuitive API design
- Extensive documentation

### Package Information
- Package ID: AzanDotNet
- Version: 1.0.0
- License: MIT
- Author: Mohamed Alkadi
- Target Framework: .NET 6.0
- NuGet Package: Available with symbols and documentation
