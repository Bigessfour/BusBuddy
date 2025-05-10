# SimpleCleaner.ps1
# A simplified script to clean build artifacts in the BusBuddy repository

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$startMessage = "[$timestamp] Starting clean build artifacts process"
Add-Content -Path "busbuddy_errors.log" -Value $startMessage
Write-Output $startMessage

# Step 1: Delete the obj and bin folders in the Tests directory
if (Test-Path -Path "Tests\obj") {
    Write-Output "Removing Tests\obj directory"
    Remove-Item -Path "Tests\obj" -Recurse -Force
    $logMsg = "[$timestamp] Successfully removed Tests\obj directory"
    Add-Content -Path "busbuddy_errors.log" -Value $logMsg
    Write-Output $logMsg
}

if (Test-Path -Path "Tests\bin") {
    Write-Output "Removing Tests\bin directory"
    Remove-Item -Path "Tests\bin" -Recurse -Force
    $logMsg = "[$timestamp] Successfully removed Tests\bin directory"
    Add-Content -Path "busbuddy_errors.log" -Value $logMsg
    Write-Output $logMsg
}

# Step 2: Restore dependencies
Write-Output "Restoring dependencies using 'dotnet restore'"
dotnet restore
$restoreExitCode = $LASTEXITCODE
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
if ($restoreExitCode -eq 0) {
    $logMsg = "[$timestamp] Successfully restored dependencies"
} else {
    $logMsg = "[$timestamp] Failed to restore dependencies with exit code $restoreExitCode"
}
Add-Content -Path "busbuddy_errors.log" -Value $logMsg
Write-Output $logMsg

# Step 3: Rebuild the solution
Write-Output "Building the solution using 'dotnet build'"
dotnet build
$buildExitCode = $LASTEXITCODE
$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
if ($buildExitCode -eq 0) {
    $logMsg = "[$timestamp] Successfully built the solution"
} else {
    $logMsg = "[$timestamp] Failed to build the solution with exit code $buildExitCode"
}
Add-Content -Path "busbuddy_errors.log" -Value $logMsg
Write-Output $logMsg

$timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
$completeMsg = "[$timestamp] Clean build artifacts process completed"
Add-Content -Path "busbuddy_errors.log" -Value $completeMsg
Write-Output $completeMsg
