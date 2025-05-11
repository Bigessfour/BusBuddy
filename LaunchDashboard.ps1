# Launch BusBuddy with dashboard
Write-Host "Starting BusBuddy with dashboard view..."

$processPath = Join-Path -Path $PSScriptRoot -ChildPath "bin\Debug\net8.0-windows\BusBuddy.exe"
Start-Process -FilePath $processPath

# Allow time for the application to start the web server
Start-Sleep -Seconds 3

# Open the dashboard in the default browser
Start-Process "https://localhost:5001/dashboard"

Write-Host "BusBuddy started and dashboard opened in browser!"
