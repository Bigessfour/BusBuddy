# Schedule a daily backup of the BusBuddy database
# This script creates a Windows Scheduled Task to run the backup script daily

$ErrorActionPreference = "Stop"

# Path to the backup script
$scriptPath = "C:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\BackupDatabase.ps1"

# Check if the backup script exists
if (-not (Test-Path $scriptPath)) {
    Write-Host "Error: Backup script not found at $scriptPath" -ForegroundColor Red
    exit 1
}

# Task name
$taskName = "BusBuddy-DatabaseBackup"

# Check if task already exists
$existingTask = Get-ScheduledTask -TaskName $taskName -ErrorAction SilentlyContinue

if ($existingTask) {
    Write-Host "The scheduled task $taskName already exists. Removing it to recreate." -ForegroundColor Yellow
    Unregister-ScheduledTask -TaskName $taskName -Confirm:$false
}

# Create the task
try {
    $action = New-ScheduledTaskAction -Execute "powershell.exe" -Argument "-NoProfile -ExecutionPolicy Bypass -File `"$scriptPath`""
    
    # Run the task daily at 2:00 AM
    $trigger = New-ScheduledTaskTrigger -Daily -At "02:00"
    
    # Run with highest privileges to ensure database access
    $principal = New-ScheduledTaskPrincipal -UserId "$env:USERDOMAIN\$env:USERNAME" -LogonType S4U -RunLevel Highest
    
    # Create the task
    $settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -RunOnlyIfNetworkAvailable -WakeToRun
    
    Register-ScheduledTask -TaskName $taskName -Action $action -Trigger $trigger -Principal $principal -Settings $settings -Description "Daily backup of BusBuddy database"
    
    Write-Host "The scheduled task $taskName has been successfully created." -ForegroundColor Green
    Write-Host "The backup will run daily at 2:00 AM."
    
    # Write to the log file
    $logMessage = "$(Get-Date) - Created scheduled task $taskName to run daily at 2:00 AM"
    Add-Content -Path "C:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\logs\backup_log.txt" -Value $logMessage
}
catch {
    Write-Host "Error creating scheduled task: $_" -ForegroundColor Red
    exit 1
}
