# Emergency-Fix-419-Errors.ps1
# This script implements a targeted fix for the 419 errors in the test build

Write-Host "Implementing emergency fix for 419 test errors..." -ForegroundColor Cyan

# Navigate to project folder
cd "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"

# 1. First ensure we have proper package references with versions
$testCsproj = Get-Content -Path "Tests\BusBuddy.Tests.csproj" -Raw

# Check if xunit has a version specified
if ($testCsproj -match '<PackageReference Include="xunit" />') {
    Write-Host "Adding version to xunit package reference..." -ForegroundColor Yellow
    $testCsproj = $testCsproj -replace '<PackageReference Include="xunit" />', '<PackageReference Include="xunit" Version="2.9.0" />'
}

if ($testCsproj -match '<PackageReference Include="Moq" />') {
    Write-Host "Adding version to Moq package reference..." -ForegroundColor Yellow
    $testCsproj = $testCsproj -replace '<PackageReference Include="Moq" />', '<PackageReference Include="Moq" Version="4.20.72" />'
}

if ($testCsproj -match '<PackageReference Include="xunit.runner.visualstudio" />') {
    Write-Host "Adding version to xunit.runner.visualstudio package reference..." -ForegroundColor Yellow
    $testCsproj = $testCsproj -replace '<PackageReference Include="xunit.runner.visualstudio" />', '<PackageReference Include="xunit.runner.visualstudio" Version="2.5.7" />'
}

# 2. Add global usings for test frameworks to make them available everywhere
if (-not ($testCsproj -match '<Using Include="Xunit"')) {
    Write-Host "Adding global using for Xunit..." -ForegroundColor Yellow
    $testCsproj = $testCsproj -replace '</ItemGroup>\r?\n</Project>', '</ItemGroup>
  <ItemGroup>
    <Using Include="Xunit" />
    <Using Include="Moq" />
    <Using Include="Xunit.Abstractions" />
    <Using Include="Microsoft.EntityFrameworkCore.InMemory" />
  </ItemGroup>
</Project>'
}

# 3. Make sure it's a proper test project
if (-not ($testCsproj -match '<IsPackable>false</IsPackable>')) {
    Write-Host "Setting IsPackable to false..." -ForegroundColor Yellow
    $testCsproj = $testCsproj -replace '<PropertyGroup>', '<PropertyGroup>
    <IsPackable>false</IsPackable>'
}

# 4. Write the updated content back
Set-Content -Path "Tests\BusBuddy.Tests.csproj" -Value $testCsproj

# 5. Let's create a helper class to handle WindowsOnlyAttribute
$windowsOnlyHelperContent = @"
using System;
using Xunit;

namespace BusBuddy.Tests
{
    [AttributeUsage(AttributeTargets.Method)]
    public class WindowsOnlyAttribute : FactAttribute
    {
        public WindowsOnlyAttribute()
        {
            if (!OperatingSystem.IsWindows())
            {
                Skip = "This test only runs on Windows";
            }
        }
    }
}
"@

# Check if file exists before creating
$windowsOnlyPath = "Tests\WindowsOnlyAttribute.cs"
if (-not (Test-Path $windowsOnlyPath)) {
    Write-Host "Creating WindowsOnlyAttribute helper class..." -ForegroundColor Yellow
    Set-Content -Path $windowsOnlyPath -Value $windowsOnlyHelperContent
}

# 6. Clean and rebuild everything
Write-Host "Cleaning solution..." -ForegroundColor Yellow
dotnet clean

Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore 

Write-Host "Building test project..." -ForegroundColor Yellow
dotnet build Tests\BusBuddy.Tests.csproj -c Debug

# 7. Building specific files that might be problematic
Write-Host "Compiling specific test files..." -ForegroundColor Yellow
dotnet build Tests\BusBuddy.Tests.csproj /p:WarningLevel=0 -c Debug

Write-Host "Fix completed. If you still see errors, please run the Fix-ProjectReference.ps1 script." -ForegroundColor Green
