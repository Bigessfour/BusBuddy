# CleanBuildArtifacts.ps1
# Script to clean build artifacts in the BusBuddy repository

# Function to append log entries to busbuddy_errors.log
function Write-BusBuddyLog {
    param (
        [string]$Message
    )
    
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "[$timestamp] $Message"
    
    try {
        Add-Content -Path "busbuddy_errors.log" -Value $logMessage
        Write-Output "LOG: $logMessage"
    }
    catch {
        Write-Error "Failed to write to log file: $_"
    }
}

# Start logging
Write-BusBuddyLog "Starting clean build artifacts process"
Write-BusBuddyLog "Current location: $(Get-Location)"

# Step 1: Delete the obj and bin folders in the Tests directory
try {
    if (Test-Path -Path "Tests\obj") {
        Write-BusBuddyLog "Removing Tests\obj directory"
        Remove-Item -Path "Tests\obj" -Recurse -Force
        Write-BusBuddyLog "Successfully removed Tests\obj directory"
    } else {
        Write-BusBuddyLog "Tests\obj directory not found, skipping"
    }

    if (Test-Path -Path "Tests\bin") {
        Write-BusBuddyLog "Removing Tests\bin directory"
        Remove-Item -Path "Tests\bin" -Recurse -Force
        Write-BusBuddyLog "Successfully removed Tests\bin directory"
    } else {
        Write-BusBuddyLog "Tests\bin directory not found, skipping"
    }
} 
catch {
    $errorMessage = "Failed to remove directories: $_"
    Write-BusBuddyLog $errorMessage
    Write-Error $errorMessage
    exit 1
}

# Step 2: Restore dependencies
try {
    Write-BusBuddyLog "Restoring dependencies using 'dotnet restore'"
    dotnet restore
    
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet restore failed with exit code $LASTEXITCODE"
    }
    
    Write-BusBuddyLog "Successfully restored dependencies"
}
catch {
    $errorMessage = "Failed to restore dependencies: $_"
    Write-BusBuddyLog $errorMessage
    Write-Error $errorMessage
    exit 1
}

# Step 3: Rebuild the solution
try {
    Write-BusBuddyLog "Building the solution using 'dotnet build'"
    dotnet build
    
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build failed with exit code $LASTEXITCODE"
    }
    
    Write-BusBuddyLog "Successfully built the solution"
}
catch {
    $errorMessage = "Failed to build the solution: $_"
    Write-BusBuddyLog $errorMessage
    Write-Error $errorMessage
    exit 1
}

Write-BusBuddyLog "Clean build artifacts process completed successfully"
