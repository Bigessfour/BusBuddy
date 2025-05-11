# RunTestsAfterFixes.ps1
# Script to run tests with the fixed MessageBoxHandler

Write-Host "Running BusBuddy tests after fixing warnings..." -ForegroundColor Cyan

# Create directory for the tests namespace if it doesn't exist
$testUtilsDir = ".\Tests\Utilities"
if (-not (Test-Path $testUtilsDir)) {
    Write-Host "Creating directory for test utilities: $testUtilsDir" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $testUtilsDir -Force | Out-Null
}

# Run the tests with specific focus on the previously problematic tests
Write-Host "Running specific tests that were previously hanging..." -ForegroundColor Green
dotnet test --logger "console;verbosity=detailed" --filter "FullyQualifiedName~VehiclesManagementForm_DataDisplayTests.LoadVehicles_ShouldHandleDatabaseException|FullyQualifiedName~RouteManagementForm_LoadingTests.LoadRoutes_ShouldHandleDatabaseException"

Write-Host "`nRunning all tests to verify nothing else is broken..." -ForegroundColor Green
dotnet test

Write-Host "`nTest run complete!" -ForegroundColor Cyan
