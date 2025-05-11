# Docker development script for BusBuddy Blazor
param (
    [switch]$Clean,
    [switch]$NoBuild
)

# Current location
$currentDir = Get-Location

# Stop and remove containers if clean is specified
if ($Clean) {
    Write-Host "Stopping and removing containers..." -ForegroundColor Yellow
    docker-compose -f docker-compose.blazor.yml down -v
}

# Build and start containers
if (!$NoBuild) {
    Write-Host "Building containers..." -ForegroundColor Green
    docker-compose -f docker-compose.blazor.yml build
}

Write-Host "Starting containers..." -ForegroundColor Green
docker-compose -f docker-compose.blazor.yml up -d

# Display container status
Write-Host "Container status:" -ForegroundColor Cyan
docker-compose -f docker-compose.blazor.yml ps

# Display access information
Write-Host "`nBusBuddy is now running in Docker!" -ForegroundColor Green
Write-Host "Access the application at: http://localhost:5000" -ForegroundColor Yellow
Write-Host "To view logs: docker-compose -f docker-compose.blazor.yml logs -f" -ForegroundColor Yellow
Write-Host "To stop: docker-compose -f docker-compose.blazor.yml down" -ForegroundColor Yellow
