# Fix-Central-Package-Management.ps1
# Script to fix the WindowsOnlyAttribute issues with Central Package Version Management

Write-Host "Fixing WindowsOnlyAttribute with Central Package Version Management..." -ForegroundColor Cyan

# 1. Make sure we're in the right directory
Set-Location -Path "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"

# 2. Define paths
$testProjectPath = "Tests\BusBuddy.Tests.csproj"
$mainProjectPath = "BusBuddy.csproj"
$attributeFilePath = "Tests\WindowsOnlyAttribute.cs"

# 3. Clean solution completely
Write-Host "Cleaning solution..." -ForegroundColor Yellow
Remove-Item -Path "bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "obj" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "Tests\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "Tests\obj" -Recurse -Force -ErrorAction SilentlyContinue

# 4. Update the Windows Only Attribute
$attributeContent = @"
using System;
using Xunit;

namespace BusBuddy.Tests
{
    /// <summary>
    /// Attribute to mark tests that only run on Windows
    /// </summary>
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

Set-Content -Path $attributeFilePath -Value $attributeContent
Write-Host "Updated WindowsOnlyAttribute implementation" -ForegroundColor Green

# 5. Fix the project file with central package management
# Remove explicit Version attributes from all <PackageReference> elements in the test project file
[xml]$projXml = Get-Content -Path $testProjectPath -Raw
$packageRefs = $projXml.Project.ItemGroup.PackageReference
foreach ($ref in $packageRefs) {
    if ($ref.Version) {
        $ref.RemoveAttribute('Version')
    }
}
$projXml.Save($testProjectPath)
Write-Host "Removed explicit Version attributes from PackageReferences in test project file for CPVM compliance" -ForegroundColor Green

# 6. Ensure the test file has proper using directives
$testFilePath = "Tests\VehiclesManagementForm_STATests.cs"
$testFileContent = Get-Content -Path $testFilePath -Raw

# Make sure it has the right using directives
if (-not ($testFileContent -match "using BusBuddy.Tests;")) {
    $testFileContent = $testFileContent -replace "using Moq;", "using Moq;`r`nusing BusBuddy.Tests;"
    Set-Content -Path $testFilePath -Value $testFileContent
    Write-Host "Added BusBuddy.Tests namespace to test file" -ForegroundColor Green
}

# 7. Restore and build
Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore

Write-Host "Building test project..." -ForegroundColor Yellow
dotnet build

Write-Host "WindowsOnlyAttribute fix process completed!" -ForegroundColor Cyan
