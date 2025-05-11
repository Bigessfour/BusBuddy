# Run BusBuddy tests in Docker
Write-Host "Building and running BusBuddy tests in Docker..." -ForegroundColor Green

# Create TestResults directory if it doesn't exist
if (-not (Test-Path -Path ".\TestResults")) {
    New-Item -Path ".\TestResults" -ItemType Directory
}

# Clean up any existing test containers
Write-Host "Cleaning up any existing test containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.tests.yml down --remove-orphans

# Build and run the tests
Write-Host "Building and running tests in Docker..." -ForegroundColor Cyan
docker-compose -f docker-compose.tests.yml up --build

# Check the exit code
if ($LASTEXITCODE -ne 0) {
    Write-Host "`nTest run encountered issues. Please check the output above for details.`n" -ForegroundColor Red
    exit $LASTEXITCODE
} else {
    Write-Host "`nTests completed successfully.`n" -ForegroundColor Green
}

# Clean up after tests
Write-Host "Cleaning up test containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.tests.yml down --remove-orphans
