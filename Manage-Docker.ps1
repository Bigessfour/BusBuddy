# Docker Start Script for BusBuddy
# This script provides a menu to manage Docker services for BusBuddy

function Show-Menu {
    Clear-Host
    Write-Host "========================================================" -ForegroundColor Cyan
    Write-Host "                 BusBuddy Docker Manager                " -ForegroundColor Cyan
    Write-Host "========================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1: Start SQL Server Docker Container" -ForegroundColor Green
    Write-Host "2: Stop SQL Server Docker Container" -ForegroundColor Yellow
    Write-Host "3: View SQL Server Container Logs" -ForegroundColor Blue
    Write-Host "4: Switch to Docker Database Mode" -ForegroundColor Green
    Write-Host "5: Switch to Local Database Mode" -ForegroundColor Green
    Write-Host "6: Test Docker Database Connection" -ForegroundColor Blue
    Write-Host "7: Rebuild Docker Container (with data reset)" -ForegroundColor Red
    Write-Host "8: View Container Status" -ForegroundColor Blue
    Write-Host "Q: Quit" -ForegroundColor Gray
    Write-Host ""
    Write-Host "--------------------------------------------------------" -ForegroundColor Cyan
}

function Start-SqlServerContainer {
    Write-Host "Starting SQL Server container..." -ForegroundColor Yellow
    docker-compose up -d sqlserver
    Write-Host "Container starting. Use option 3 to view logs." -ForegroundColor Green
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

function Stop-SqlServerContainer {
    Write-Host "Stopping SQL Server container..." -ForegroundColor Yellow
    docker-compose down
    Write-Host "Container stopped." -ForegroundColor Green
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

function Show-SqlServerLogs {
    Write-Host "Displaying SQL Server container logs (press Ctrl+C to exit):" -ForegroundColor Yellow
    docker-compose logs -f sqlserver
}

function Switch-ToDatabaseMode {
    param (
        [string]$Mode
    )
    
    Write-Host "Switching to $Mode database mode..." -ForegroundColor Yellow
    & .\Switch-DatabaseMode.ps1 -Mode $Mode
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

function Test-DockerDbConnection {
    Write-Host "Testing Docker database connection..." -ForegroundColor Yellow
    & .\Test-DockerDatabase.ps1
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

function Reset-DockerContainer {
    Write-Host "WARNING: This will RESET all database data in the Docker container!" -ForegroundColor Red
    Write-Host "Are you sure you want to continue? (y/n)" -ForegroundColor Yellow
    $response = Read-Host
    
    if ($response.ToLower() -eq "y") {
        Write-Host "Removing container and rebuilding..." -ForegroundColor Yellow
        docker-compose down
        docker volume rm busbuddy-sqlserver-data
        docker-compose up -d sqlserver
        Write-Host "Container has been rebuilt with a fresh database." -ForegroundColor Green
    }
    else {
        Write-Host "Operation cancelled." -ForegroundColor Green
    }
    
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

function Show-ContainerStatus {
    Write-Host "Container Status:" -ForegroundColor Yellow
    docker ps -a --filter "name=busbuddy"
    
    Write-Host "`nVolume Status:" -ForegroundColor Yellow
    docker volume ls --filter "name=busbuddy"
    
    Write-Host "Press any key to continue..."
    [void][System.Console]::ReadKey($true)
}

# Check if Docker is available
try {
    $dockerStatus = docker --version
    Write-Host "Docker detected: $dockerStatus" -ForegroundColor Green
}
catch {
    Write-Host "Docker is not available. Please install Docker Desktop first." -ForegroundColor Red
    Write-Host "Press any key to exit..."
    [void][System.Console]::ReadKey($true)
    exit
}

# Main menu loop
$selection = ""
do {
    Show-Menu
    $selection = Read-Host "Enter your choice"
    
    switch ($selection) {
        "1" { Start-SqlServerContainer }
        "2" { Stop-SqlServerContainer }
        "3" { Show-SqlServerLogs }
        "4" { Switch-ToDatabaseMode -Mode "docker" }
        "5" { Switch-ToDatabaseMode -Mode "local" }
        "6" { Test-DockerDbConnection }
        "7" { Reset-DockerContainer }
        "8" { Show-ContainerStatus }
        "q" { return }
        default { 
            Write-Host "Invalid selection. Press any key to continue..."
            [void][System.Console]::ReadKey($true)
        }
    }
} while ($selection -ne "q")
