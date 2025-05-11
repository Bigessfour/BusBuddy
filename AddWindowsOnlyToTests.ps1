# This script adds the [WindowsOnly] attribute to all UI test methods in the BusBuddy test suite
# It identifies tests that use Windows Forms components and tags them appropriately for containerization

$testDir = Join-Path $PSScriptRoot "Tests"
$uiTestFiles = Get-ChildItem -Path $testDir -Filter "*Form*.cs" -Recurse

Write-Host "Adding WindowsOnly attributes to UI test files..." -ForegroundColor Green

foreach ($file in $uiTestFiles) {
    $content = Get-Content -Path $file.FullName -Raw
    
    # Skip files that are already using WindowsOnly trait
    if ($content -match "WindowsOnly") {
        Write-Host "Skipping $($file.Name) - already has WindowsOnly attribute" -ForegroundColor Yellow
        continue
    }
    
    # Make sure the file contains test methods
    if (-not ($content -match "\[Fact\]" -or $content -match "\[Theory\]")) {
        Write-Host "Skipping $($file.Name) - no test methods found" -ForegroundColor Yellow
        continue
    }
    
    # Add the using statement if needed
    if (-not ($content -match "using BusBuddy.Tests.Utilities;")) {
        $content = $content -replace "using System;", "using System;`r`nusing BusBuddy.Tests.Utilities;"
    }
    
    # Replace [Fact] attributes with [Fact, WindowsOnly] and [Theory] with [Theory, WindowsOnly]
    $content = $content -replace "\[Fact\]", "[Fact, WindowsOnly]"
    $content = $content -replace "\[Theory\]", "[Theory, WindowsOnly]"
    
    # Write the updated content back to the file
    Set-Content -Path $file.FullName -Value $content
    
    Write-Host "Updated $($file.Name) - added WindowsOnly attributes" -ForegroundColor Green
}

Write-Host "`nCompleted adding WindowsOnly attributes to test files" -ForegroundColor Green
