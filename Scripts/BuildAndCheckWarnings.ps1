# BuildAndCheckWarnings.ps1
# Script to build the solution and check for warnings

Write-Host "Cleaning and building BusBuddy solution..." -ForegroundColor Cyan

# Clean the solution
Write-Host "Cleaning solution..." -ForegroundColor Yellow
$cleanOutput = & dotnet clean

# Build the solution with detailed output
Write-Host "Building solution..." -ForegroundColor Green
$buildOutput = & dotnet build /warnaserror-

# Display the results
Write-Host "`nBuild Results:" -ForegroundColor Cyan
Write-Host $buildOutput

Write-Host "`nBuild process complete!" -ForegroundColor Cyan
