# Database backup script for BusBuddy
# Create a backup of the BusBuddy database

$ErrorActionPreference = "Stop"

# Create backup directory if it doesn't exist
$backupDir = "C:\Backups"
if (-not (Test-Path $backupDir)) {
    Write-Host "Creating backup directory: $backupDir"
    New-Item -Path $backupDir -ItemType Directory | Out-Null
}

# Generate a backup filename with date
$date = Get-Date -Format "yyyyMMdd"
$backupPath = "$backupDir\BusBuddy_$date.bak"

Write-Host "Backing up BusBuddy database to $backupPath"

try {
    # Import SQL Server module if available
    if (Get-Module -ListAvailable -Name SqlServer) {
        Import-Module SqlServer
        
        # Backup the database using Invoke-SqlCmd
        $query = "BACKUP DATABASE BusBuddy TO DISK='$backupPath' WITH INIT, COMPRESSION"
        Invoke-Sqlcmd -ServerInstance "localhost\SQLEXPRESS" -Query $query -ErrorAction Stop
        
        Write-Host "Database backup completed successfully." -ForegroundColor Green
    }
    else {
        # Fallback to sqlcmd.exe if module not available
        Write-Host "SqlServer module not found. Using sqlcmd.exe instead."
        
        $query = "BACKUP DATABASE BusBuddy TO DISK='$backupPath' WITH INIT, COMPRESSION"
        sqlcmd -S "localhost\SQLEXPRESS" -Q $query
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Database backup completed successfully." -ForegroundColor Green
        }
        else {
            throw "sqlcmd exited with code $LASTEXITCODE"
        }
    }
    
    # Log the successful backup
    Add-Content -Path "C:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\logs\backup_log.txt" -Value "$(Get-Date) - Backup created: $backupPath"
}
catch {
    Write-Host "Error backing up database: $_" -ForegroundColor Red
    Add-Content -Path "C:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\logs\backup_log.txt" -Value "$(Get-Date) - Backup failed: $_"
    exit 1
}

# Cleanup old backups (keep only last 5 backups)
Write-Host "Cleaning up old backups (keeping 5 most recent)..."
Get-ChildItem -Path $backupDir -Filter "BusBuddy_*.bak" | 
    Sort-Object LastWriteTime -Descending | 
    Select-Object -Skip 5 | 
    ForEach-Object {
        Write-Host "Removing old backup: $($_.Name)"
        Remove-Item $_.FullName -Force
    }
