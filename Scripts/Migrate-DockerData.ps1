# Export-Import Database Script for BusBuddy
# This script helps migrate data between local SQL Server and Docker SQL Server

param (
    [Parameter(Mandatory=$true)]
    [ValidateSet("export-to-docker", "import-from-docker")]
    [string]$Operation
)

$ErrorActionPreference = "Stop"

# Configuration
$localServer = "localhost\SQLEXPRESS"
$localDatabase = "BusBuddy"
$localAuth = "Windows"

$dockerServer = "localhost,1433"
$dockerDatabase = "BusBuddy"
$dockerUser = "BusBuddyApp" 
$dockerPassword = "App@P@ss!2025"

$tempDir = "C:\Temp\BusBuddyMigration"
$backupFile = "$tempDir\BusBuddy_Migration.bacpac"

# Create temp directory if it doesn't exist
if (-not (Test-Path $tempDir)) {
    New-Item -Path $tempDir -ItemType Directory -Force | Out-Null
    Write-Host "Created temporary directory: $tempDir" -ForegroundColor Yellow
}

function Test-CommandExists {
    param ($command)
    
    $exists = $null -ne (Get-Command $command -ErrorAction SilentlyContinue)
    if (-not $exists) {
        Write-Host "❌ Required tool '$command' not found." -ForegroundColor Red
        Write-Host "Please install SQL Server Management Studio or SQL Server Data Tools." -ForegroundColor Yellow
        exit 1
    }
    return $exists
}

function Export-LocalToDocker {
    Write-Host "🔄 Exporting data from local SQL Server to Docker..." -ForegroundColor Cyan
    
    # Check for required tools
    Test-CommandExists "SqlPackage"
    
    try {
        # Step 1: Export local database to BACPAC
        Write-Host "📤 Exporting local database to BACPAC..." -ForegroundColor Yellow
        
        SqlPackage /Action:Export `
            /SourceServerName:$localServer `
            /SourceDatabaseName:$localDatabase `
            /TargetFile:$backupFile
        
        if (-not $?) {
            throw "Failed to export database to BACPAC"
        }
        
        Write-Host "✅ Successfully exported local database to $backupFile" -ForegroundColor Green
        
        # Step 2: Import BACPAC to Docker SQL Server
        Write-Host "📥 Importing BACPAC to Docker SQL Server..." -ForegroundColor Yellow
        
        SqlPackage /Action:Import `
            /TargetServerName:$dockerServer `
            /TargetDatabaseName:$dockerDatabase `
            /TargetUser:$dockerUser `
            /TargetPassword:$dockerPassword `
            /SourceFile:$backupFile
        
        if (-not $?) {
            throw "Failed to import database to Docker SQL Server"
        }
        
        Write-Host "✅ Successfully imported data to Docker SQL Server" -ForegroundColor Green
        Write-Host "🎉 Data migration from local to Docker completed!" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Error during export-import process: $_" -ForegroundColor Red
        exit 1
    }
}

function Import-DockerToLocal {
    Write-Host "🔄 Importing data from Docker SQL Server to local..." -ForegroundColor Cyan
    
    # Check for required tools
    Test-CommandExists "SqlPackage"
    
    try {
        # Step 1: Export Docker database to BACPAC
        Write-Host "📤 Exporting Docker database to BACPAC..." -ForegroundColor Yellow
        
        SqlPackage /Action:Export `
            /SourceServerName:$dockerServer `
            /SourceDatabaseName:$dockerDatabase `
            /SourceUser:$dockerUser `
            /SourcePassword:$dockerPassword `
            /TargetFile:$backupFile
        
        if (-not $?) {
            throw "Failed to export database from Docker"
        }
        
        Write-Host "✅ Successfully exported Docker database to $backupFile" -ForegroundColor Green
        
        # Step 2: Import BACPAC to local SQL Server
        Write-Host "📥 Importing BACPAC to local SQL Server..." -ForegroundColor Yellow
        
        SqlPackage /Action:Import `
            /TargetServerName:$localServer `
            /TargetDatabaseName:$localDatabase `
            /SourceFile:$backupFile
        
        if (-not $?) {
            throw "Failed to import database to local SQL Server"
        }
        
        Write-Host "✅ Successfully imported data to local SQL Server" -ForegroundColor Green
        Write-Host "🎉 Data migration from Docker to local completed!" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Error during import-export process: $_" -ForegroundColor Red
        exit 1
    }
}

# Main execution
switch ($Operation) {
    "export-to-docker" {
        Export-LocalToDocker
    }
    "import-from-docker" {
        Import-DockerToLocal
    }
}
