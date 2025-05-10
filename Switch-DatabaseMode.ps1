# This script helps switch between Docker and local database modes
# for the BusBuddy application
param (
    [Parameter(Mandatory=$true)]
    [ValidateSet("docker", "local")]
    [string]$Mode
)

# Function to set environment variable permanently
function Set-EnvironmentVariable {
    param (
        [string]$Name,
        [string]$Value
    )
    
    # Set for current session
    [Environment]::SetEnvironmentVariable($Name, $Value, [EnvironmentVariableTarget]::Process)
    
    # Set for user (persists across sessions)
    [Environment]::SetEnvironmentVariable($Name, $Value, [EnvironmentVariableTarget]::User)
    
    Write-Host "Environment variable $Name set to $Value for current user" -ForegroundColor Green
}

# Function to check Docker status
function Test-Docker {
    try {
        $dockerInfo = docker info 2>&1
        if ($LASTEXITCODE -eq 0) {
            return $true
        }
        return $false
    }
    catch {
        return $false
    }
}

switch ($Mode) {
    "docker" {
        # Check if Docker is running
        if (-not (Test-Docker)) {
            Write-Host "Docker is not running. Please start Docker Desktop first." -ForegroundColor Red
            exit 1
        }
        
        # Set environment variable for Docker DB
        Set-EnvironmentVariable -Name "USE_DOCKER_DB" -Value "true"
        
        # Start the SQL Server container if not already running
        $containerRunning = docker ps | Select-String "busbuddy-sqlserver"
        if (-not $containerRunning) {
            Write-Host "Starting SQL Server Docker container..." -ForegroundColor Yellow
            docker-compose up -d sqlserver
            
            # Wait a bit for the container to start
            Write-Host "Waiting for SQL Server to be ready..." -ForegroundColor Yellow
            Start-Sleep -Seconds 15
        }
        else {
            Write-Host "SQL Server Docker container is already running." -ForegroundColor Green
        }
        
        Write-Host "BusBuddy is now set to use the Docker database!" -ForegroundColor Cyan
        Write-Host "Connection string: DockerConnection" -ForegroundColor Cyan
        Write-Host "Database server: localhost,1433" -ForegroundColor Cyan
    }
    "local" {
        # Set environment variable for local DB
        Set-EnvironmentVariable -Name "USE_DOCKER_DB" -Value "false"
        
        Write-Host "BusBuddy is now set to use the local database!" -ForegroundColor Cyan
        Write-Host "Connection string: DefaultConnection" -ForegroundColor Cyan
        Write-Host "Database server: localhost\SQLEXPRESS" -ForegroundColor Cyan
    }
}

# Display current connection info
Write-Host "`nCurrent Database Connection Mode: $Mode" -ForegroundColor White -BackgroundColor DarkBlue
