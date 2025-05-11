# Start BusBuddy Dashboard in Docker Desktop
# This script is specifically for running and testing the dashboard interface

Write-Host "Starting BusBuddy Dashboard for development and testing..." -ForegroundColor Green

# Check if Docker Desktop is running
try {
    $null = docker info
}
catch {
    Write-Host "ERROR: Docker Desktop is not running. Please start Docker Desktop and try again." -ForegroundColor Red
    exit 1
}

# Parse arguments
$rebuild = $false
foreach ($arg in $args) {
    if ($arg -eq "--rebuild") {
        $rebuild = $true
    }
}

# Stop existing containers
Write-Host "Stopping any running dashboard containers..." -ForegroundColor Yellow
docker-compose -f docker-compose.dashboard.yml down

# Clean up if rebuild requested
if ($rebuild) {
    Write-Host "Removing old container images..." -ForegroundColor Yellow
    docker rmi busbuddy-blazor-dashboard -f 2>$null
}

# Build and start containers
Write-Host "Building and starting BusBuddy Dashboard..." -ForegroundColor Cyan
docker-compose -f docker-compose.dashboard.yml up --build -d

# Wait for containers to initialize
Write-Host "Waiting for services to initialize..." -ForegroundColor Yellow
Start-Sleep -Seconds 5

# Check if containers are running
$containerStatus = docker-compose -f docker-compose.dashboard.yml ps
if ($containerStatus -match "running") {
    Write-Host "`nBusBuddy Dashboard is now running!" -ForegroundColor Green
    
    # Try to open the dashboard in default browser
    Write-Host "Opening dashboard in your default browser..." -ForegroundColor Cyan
    Start-Process "http://localhost:5500/modern-dashboard"
    
    Write-Host "`nAvailable URLs:" -ForegroundColor White
    Write-Host "- Modern Dashboard: http://localhost:5500/modern-dashboard" -ForegroundColor Green
    Write-Host "- Classic Dashboard: http://localhost:5500/dashboard" -ForegroundColor Green
    Write-Host "`nDatabase Connection:" -ForegroundColor White
    Write-Host "- Server: localhost,1433" -ForegroundColor Cyan
    Write-Host "- Username: BusBuddyApp" -ForegroundColor Cyan
    Write-Host "- Password: App@P@ss!2025" -ForegroundColor Cyan
    
    Write-Host "`nCommands:" -ForegroundColor White
    Write-Host "- View logs: docker-compose -f docker-compose.dashboard.yml logs -f" -ForegroundColor Cyan
    Write-Host "- Stop dashboard: docker-compose -f docker-compose.dashboard.yml down" -ForegroundColor Cyan
}
else {
    Write-Host "`nERROR: Dashboard containers failed to start properly." -ForegroundColor Red
    Write-Host "Check logs with: docker-compose -f docker-compose.dashboard.yml logs" -ForegroundColor Yellow
}
