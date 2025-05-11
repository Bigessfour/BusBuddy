# This script tests the connection to the Docker SQL Server database for BusBuddy
# It helps diagnose connection issues and validates that the Docker database is working correctly.

$ErrorActionPreference = "Stop"

# Define connection parameters
$server = "localhost,1433"
$database = "BusBuddy"
$username = "BusBuddyApp"
$password = "App@P@ss!2025"

# Function to check if Docker is running
function Test-Docker {
    try {
        $dockerStatus = docker ps 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✓ Docker is running" -ForegroundColor Green
            return $true
        }
        else {
            Write-Host "✗ Docker is not running" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "✗ Docker is not installed or not running" -ForegroundColor Red
        return $false
    }
}

# Function to check if the container is running
function Test-DockerContainer {
    param (
        [string]$ContainerName = "busbuddy-sqlserver"
    )
    
    try {
        $container = docker ps --filter "name=$ContainerName" --format "{{.Names}}" 2>&1
        if ($LASTEXITCODE -eq 0 -and $container -eq $ContainerName) {
            Write-Host "✓ Container '$ContainerName' is running" -ForegroundColor Green
            return $true
        }
        else {
            Write-Host "✗ Container '$ContainerName' is not running" -ForegroundColor Red
            return $false
        }
    }
    catch {
        Write-Host "✗ Error checking Docker container" -ForegroundColor Red
        return $false
    }
}

# Function to test SQL connection
function Test-SqlConnection {
    param (
        [string]$Server,
        [string]$Database,
        [string]$Username,
        [string]$Password
    )
    
    try {
        # Check if SqlServer module is available
        if (Get-Module -ListAvailable -Name SqlServer) {
            Write-Host "Testing SQL connection using SqlServer module..." -ForegroundColor Yellow
            
            # Create a connection string
            $connectionString = "Server=$Server;Database=$Database;User Id=$Username;Password=$Password;TrustServerCertificate=True;Encrypt=False"
            
            # Create connection object
            $connection = New-Object System.Data.SqlClient.SqlConnection
            $connection.ConnectionString = $connectionString
            
            # Try to open connection
            $connection.Open()
            
            if ($connection.State -eq 'Open') {
                Write-Host "✓ Successfully connected to SQL Server" -ForegroundColor Green
                
                # Execute a test query
                $command = $connection.CreateCommand()
                $command.CommandText = "SELECT @@VERSION"
                $result = $command.ExecuteScalar()
                
                Write-Host "✓ SQL Server version: $result" -ForegroundColor Green
                
                # Get database information
                $command.CommandText = "SELECT DB_NAME(), SUSER_NAME()"
                $reader = $command.ExecuteReader()
                if ($reader.Read()) {
                    Write-Host "✓ Connected to database: $($reader[0]) as user: $($reader[1])" -ForegroundColor Green
                }
                $reader.Close()
                
                # Close the connection
                $connection.Close()
                return $true
            }
        }
        else {
            Write-Host "SqlServer module not found. Using sqlcmd.exe instead..." -ForegroundColor Yellow
            
            # Test connection using sqlcmd
            $query = "SELECT @@VERSION"
            $result = sqlcmd -S $Server -d $Database -U $Username -P $Password -Q $query -h -1
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "✓ Successfully connected to SQL Server" -ForegroundColor Green
                Write-Host "✓ SQL Server version: $result" -ForegroundColor Green
                return $true
            }
        }
    }
    catch {
        Write-Host "✗ Failed to connect to SQL Server: $_" -ForegroundColor Red
        return $false
    }
    
    return $false
}

# Main testing sequence
Write-Host "BusBuddy Docker Database Connection Test" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan

# Step 1: Check if Docker is running
$dockerRunning = Test-Docker
if (-not $dockerRunning) {
    Write-Host "Please start Docker Desktop and try again." -ForegroundColor Yellow
    exit 1
}

# Step 2: Check if the SQL Server container is running
$containerRunning = Test-DockerContainer
if (-not $containerRunning) {
    Write-Host "Would you like to start the container now? (Y/N)" -ForegroundColor Yellow
    $response = Read-Host
    if ($response -eq "Y" -or $response -eq "y") {
        Write-Host "Starting Docker container..." -ForegroundColor Yellow
        docker-compose up -d sqlserver
        Start-Sleep -Seconds 20 # Wait for container to start
        $containerRunning = Test-DockerContainer
        if (-not $containerRunning) {
            Write-Host "Failed to start the container. Please check Docker logs." -ForegroundColor Red
            exit 1
        }
    }
    else {
        Write-Host "Exiting test. Please start the container and try again." -ForegroundColor Yellow
        exit 1
    }
}

# Step 3: Test SQL connection
Write-Host "`nTesting connection to SQL Server..." -ForegroundColor Yellow
Write-Host "Server: $server" -ForegroundColor Yellow
Write-Host "Database: $database" -ForegroundColor Yellow
Write-Host "Username: $username" -ForegroundColor Yellow

$connectionSuccessful = Test-SqlConnection -Server $server -Database $database -Username $username -Password $password

if ($connectionSuccessful) {
    Write-Host "`n✓ All tests passed! Your Docker SQL Server is working correctly." -ForegroundColor Green
    Write-Host "You can switch to Docker mode with: .\Switch-DatabaseMode.ps1 -Mode docker" -ForegroundColor Green
}
else {
    Write-Host "`n✗ Connection test failed. Please check the Docker container logs:" -ForegroundColor Red
    Write-Host "docker-compose logs sqlserver" -ForegroundColor Yellow
    
    Write-Host "`nTroubleshooting tips:" -ForegroundColor Yellow
    Write-Host "1. Make sure the SQL Server container is healthy:" -ForegroundColor Yellow
    Write-Host "   docker inspect --format ""{{.State.Health.Status}}"" busbuddy-sqlserver" -ForegroundColor DarkGray
    
    Write-Host "2. Check if the initialization script ran successfully:" -ForegroundColor Yellow
    Write-Host "   docker exec -it busbuddy-sqlserver ls /docker-entrypoint-initdb.d" -ForegroundColor DarkGray
    
    Write-Host "3. Try running the initialization script manually:" -ForegroundColor Yellow
    Write-Host "   docker exec -it busbuddy-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ""BusB#ddy!2025"" -i /docker-entrypoint-initdb.d/init-db.sql" -ForegroundColor DarkGray
    
    Write-Host "4. See TROUBLESHOOTING.md for more details on resolving Docker-related issues" -ForegroundColor Yellow
}
