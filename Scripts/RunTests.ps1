# RunTests.ps1
# Script to run tests with the new MessageBoxHandler feature

Write-Host "Running BusBuddy tests with MessageBoxHandler..." -ForegroundColor Cyan

# Create directory for the tests namespace if it doesn't exist
$testUtilsDir = ".\Tests\Utilities"
if (-not (Test-Path $testUtilsDir)) {
    Write-Host "Creating directory for test utilities: $testUtilsDir" -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $testUtilsDir -Force | Out-Null
}

# Run the tests
Write-Host "Running tests..." -ForegroundColor Green
dotnet test --logger "console;verbosity=detailed" --filter "FullyQualifiedName~VehiclesManagementForm_DataDisplayTests|FullyQualifiedName~RouteManagementForm_LoadingTests"

Write-Host "Test run complete!" -ForegroundColor Cyan
