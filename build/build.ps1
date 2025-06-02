# AzanDotNet Build Script
param(
    [string]$Configuration = "Release",
    [string]$Version = "1.0.0",
    [switch]$Pack,
    [switch]$Test,
    [switch]$Clean
)

Write-Host "=== AzanDotNet Build Script ===" -ForegroundColor Green
Write-Host "Configuration: $Configuration" -ForegroundColor Yellow
Write-Host "Version: $Version" -ForegroundColor Yellow

$SolutionPath = Join-Path $PSScriptRoot ".." "AzanDotNet.sln"
$ProjectPath = Join-Path $PSScriptRoot ".." "src" "AzanDotNet" "AzanDotNet.csproj"

# Clean if requested
if ($Clean) {
    Write-Host "Cleaning solution..." -ForegroundColor Blue
    dotnet clean $SolutionPath --configuration $Configuration
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Clean failed"
        exit 1
    }
}

# Restore packages
Write-Host "Restoring packages..." -ForegroundColor Blue
dotnet restore $SolutionPath
if ($LASTEXITCODE -ne 0) {
    Write-Error "Restore failed"
    exit 1
}

# Build solution
Write-Host "Building solution..." -ForegroundColor Blue
dotnet build $SolutionPath --configuration $Configuration --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed"
    exit 1
}

# Run tests if requested
if ($Test) {
    Write-Host "Running tests..." -ForegroundColor Blue
    dotnet test $SolutionPath --configuration $Configuration --no-build
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Tests failed"
        exit 1
    }
}

# Pack if requested
if ($Pack) {
    Write-Host "Creating NuGet package..." -ForegroundColor Blue
    dotnet pack $ProjectPath --configuration $Configuration --no-build --output "artifacts" -p:PackageVersion=$Version
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Pack failed"
        exit 1
    }
    
    Write-Host "Package created successfully!" -ForegroundColor Green
    $PackagePath = Join-Path $PSScriptRoot ".." "artifacts" "AzanDotNet.$Version.nupkg"
    if (Test-Path $PackagePath) {
        Write-Host "Package location: $PackagePath" -ForegroundColor Yellow
    }
}

Write-Host "Build completed successfully!" -ForegroundColor Green
