# Force-UninstallDockerDesktop.ps1
# Script to completely uninstall Docker Desktop from Windows
# Created: May 10, 2025

# Ensure script is running as administrator
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Warning "This script requires administrative privileges. Please run as Administrator."
    Write-Host "Right-click on PowerShell and select 'Run as Administrator', then run this script again."
    exit
}

Write-Host "Starting Docker Desktop force uninstallation process..." -ForegroundColor Cyan

# Stop any Docker related services and processes
Write-Host "Stopping Docker processes and services..." -ForegroundColor Yellow
$services = @("com.docker.service", "docker", "docker-credential-wincred", "DockerDesktopVM", "vmms")
foreach ($service in $services) {
    if (Get-Service $service -ErrorAction SilentlyContinue) {
        Stop-Service -Name $service -Force -ErrorAction SilentlyContinue
        Write-Host "  - Stopped service: $service" -ForegroundColor Green
    }
}

# Kill Docker processes
$processes = @("Docker Desktop", "Docker Desktop.exe", "Docker Desktop", "com.docker.proxy", "com.docker.service", "com.docker.backend", "dockerd")
foreach ($process in $processes) {
    Get-Process | Where-Object { $_.ProcessName -like "*$process*" } | ForEach-Object {
        Stop-Process -Id $_.Id -Force -ErrorAction SilentlyContinue
        Write-Host "  - Terminated process: $($_.ProcessName)" -ForegroundColor Green
    }
}

# Uninstall Docker Desktop application via standard method first
Write-Host "Attempting standard uninstallation..." -ForegroundColor Yellow
$installedApplications = Get-WmiObject -Class Win32_Product | Where-Object { $_.Name -like "*Docker Desktop*" }

if ($installedApplications) {
    foreach ($app in $installedApplications) {
        try {
            Write-Host "Uninstalling $($app.Name)..."
            $app.Uninstall() | Out-Null
            Write-Host "  - Standard uninstall of $($app.Name) completed" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to uninstall using standard method: $($_.Exception.Message)" -ForegroundColor Yellow
        }
    }
}
else {
    Write-Host "  - Docker Desktop not found in installed applications" -ForegroundColor Yellow
}

# Try alternative uninstallation method using registry
Write-Host "Searching for Docker Desktop in registry uninstall entries..." -ForegroundColor Yellow
$uninstallKeys = @(
    "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*",
    "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\*",
    "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\*"
)

foreach ($key in $uninstallKeys) {
    $dockerApps = Get-ItemProperty -Path $key -ErrorAction SilentlyContinue | Where-Object { $_.DisplayName -like "*Docker Desktop*" }
    
    if ($dockerApps) {
        foreach ($app in $dockerApps) {
            try {
                if ($app.UninstallString) {
                    $uninstallCmd = $app.UninstallString
                    
                    # If the uninstall string contains msiexec
                    if ($uninstallCmd -match "msiexec") {
                        # Extract the product code
                        $productCode = $uninstallCmd -replace '.*({[A-Z0-9\-]+}).*', '$1'
                        if ($productCode -match '{[A-Z0-9\-]+}') {
                            Write-Host "Running: msiexec.exe /x $productCode /qn" -ForegroundColor Yellow
                            Start-Process "msiexec.exe" -ArgumentList "/x $productCode /qn" -Wait
                            Write-Host "  - Uninstalled Docker Desktop using Product Code: $productCode" -ForegroundColor Green
                        }
                    }
                    else {
                        # For non-MSI uninstallers, add silent flags
                        $uninstallCmd = "$uninstallCmd /silent"
                        Write-Host "Running: $uninstallCmd" -ForegroundColor Yellow
                        
                        # Execute the uninstall command
                        $uninstallCmdParts = $uninstallCmd -split ' ', 2
                        $executable = $uninstallCmdParts[0].Trim('"')
                        $arguments = if ($uninstallCmdParts.Count -gt 1) { $uninstallCmdParts[1] } else { "" }
                        
                        Start-Process -FilePath $executable -ArgumentList $arguments -Wait
                        Write-Host "  - Uninstalled Docker Desktop using direct uninstaller" -ForegroundColor Green
                    }
                }
            }
            catch {
                Write-Host "  - Failed to uninstall from registry: $($_.Exception.Message)" -ForegroundColor Red
            }
        }
    }
}

# Remove Docker related folders
Write-Host "Removing Docker related folders..." -ForegroundColor Yellow
$foldersToRemove = @(
    "$env:ProgramFiles\Docker",
    "$env:ProgramFiles\Docker Desktop",
    "$env:ProgramFiles\Docker Desktop Installer",
    "$env:ProgramData\Docker",
    "$env:ProgramData\DockerDesktop",
    "$env:USERPROFILE\.docker",
    "$env:LOCALAPPDATA\Docker",
    "$env:APPDATA\Docker",
    "$env:APPDATA\DockerDesktop",
    "$env:LOCALAPPDATA\Docker Desktop",
    "$env:ProgramW6432\Docker"
)

foreach ($folder in $foldersToRemove) {
    if (Test-Path $folder) {
        try {
            Remove-Item -Path $folder -Recurse -Force -ErrorAction SilentlyContinue
            Write-Host "  - Removed folder: $folder" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to remove folder $folder. Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Remove Docker Desktop settings from registry
Write-Host "Removing Docker related registry entries..." -ForegroundColor Yellow
$registryKeysToRemove = @(
    "HKCU:\SOFTWARE\Docker Inc.",
    "HKLM:\SOFTWARE\Docker Inc.",
    "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Docker Desktop",
    "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Docker Desktop",
    "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Docker Desktop"
)

foreach ($key in $registryKeysToRemove) {
    if (Test-Path $key) {
        try {
            Remove-Item -Path $key -Recurse -Force -ErrorAction SilentlyContinue
            Write-Host "  - Removed registry key: $key" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to remove registry key $key. Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Check for and remove Docker Desktop scheduled tasks
Write-Host "Removing Docker scheduled tasks..." -ForegroundColor Yellow
$scheduledTasks = Get-ScheduledTask | Where-Object { $_.TaskName -like "*Docker*" } -ErrorAction SilentlyContinue

if ($scheduledTasks) {
    foreach ($task in $scheduledTasks) {
        try {
            Unregister-ScheduledTask -TaskName $task.TaskName -Confirm:$false -ErrorAction SilentlyContinue
            Write-Host "  - Removed scheduled task: $($task.TaskName)" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to remove scheduled task $($task.TaskName). Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Remove Docker Desktop shortcut
$shortcuts = @(
    "$env:USERPROFILE\Desktop\Docker Desktop.lnk",
    "$env:APPDATA\Microsoft\Windows\Start Menu\Programs\Docker\Docker Desktop.lnk",
    "$env:ProgramData\Microsoft\Windows\Start Menu\Programs\Docker\Docker Desktop.lnk"
)

foreach ($shortcut in $shortcuts) {
    if (Test-Path $shortcut) {
        try {
            Remove-Item -Path $shortcut -Force -ErrorAction SilentlyContinue
            Write-Host "  - Removed shortcut: $shortcut" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to remove shortcut $shortcut. Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Remove Docker from Windows Firewall rules
Write-Host "Removing Docker Windows Firewall rules..." -ForegroundColor Yellow
$firewallRules = Get-NetFirewallRule -DisplayName "*Docker*" -ErrorAction SilentlyContinue
if ($firewallRules) {
    foreach ($rule in $firewallRules) {
        try {
            Remove-NetFirewallRule -ID $rule.Name -ErrorAction SilentlyContinue
            Write-Host "  - Removed firewall rule: $($rule.DisplayName)" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to remove firewall rule $($rule.DisplayName). Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Remove Docker Desktop WSL integration
Write-Host "Removing Docker WSL integration..." -ForegroundColor Yellow
$wslDistros = wsl --list --quiet 2>$null

if ($LASTEXITCODE -eq 0) {
    $dockerDistros = $wslDistros | Where-Object { $_ -like "*docker*" }
    foreach ($distro in $dockerDistros) {
        if ($distro.Trim() -ne "") {
            try {
                wsl --unregister $distro.Trim() 2>$null
                Write-Host "  - Unregistered WSL distro: $distro" -ForegroundColor Green
            }
            catch {
                Write-Host "  - Failed to unregister WSL distro $distro. Error: $($_.Exception.Message)" -ForegroundColor Red
            }
        }
    }
}

# Restart Docker-related Windows services that might be affected
Write-Host "Restarting Windows services that might have been affected..." -ForegroundColor Yellow
$servicesToRestart = @("vmms", "HvHost")
foreach ($service in $servicesToRestart) {
    if (Get-Service $service -ErrorAction SilentlyContinue) {
        try {
            Restart-Service -Name $service -Force -ErrorAction SilentlyContinue
            Write-Host "  - Restarted service: $service" -ForegroundColor Green
        }
        catch {
            Write-Host "  - Failed to restart service $service. Error: $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Clean up system environment variables related to Docker
Write-Host "Cleaning up Docker environment variables..." -ForegroundColor Yellow
$envVars = @("DOCKER_HOST", "DOCKER_CERT_PATH", "DOCKER_CONFIG", "DOCKER_TOOLBOX_INSTALL_PATH", "DOCKER_MACHINE_NAME")

foreach ($var in $envVars) {
    if ([System.Environment]::GetEnvironmentVariable($var, "Machine")) {
        [System.Environment]::SetEnvironmentVariable($var, $null, "Machine")
        Write-Host "  - Removed system environment variable: $var" -ForegroundColor Green
    }
    
    if ([System.Environment]::GetEnvironmentVariable($var, "User")) {
        [System.Environment]::SetEnvironmentVariable($var, $null, "User")
        Write-Host "  - Removed user environment variable: $var" -ForegroundColor Green
    }
}

# Update PATH environment variable to remove Docker-related paths
$userPath = [System.Environment]::GetEnvironmentVariable("PATH", "User")
$machinePath = [System.Environment]::GetEnvironmentVariable("PATH", "Machine")

if ($userPath) {
    $newUserPath = ($userPath -split ';' | Where-Object { $_ -notmatch 'Docker' }) -join ';'
    if ($userPath -ne $newUserPath) {
        [System.Environment]::SetEnvironmentVariable("PATH", $newUserPath, "User")
        Write-Host "  - Removed Docker paths from user PATH variable" -ForegroundColor Green
    }
}

if ($machinePath) {
    $newMachinePath = ($machinePath -split ';' | Where-Object { $_ -notmatch 'Docker' }) -join ';'
    if ($machinePath -ne $newMachinePath) {
        [System.Environment]::SetEnvironmentVariable("PATH", $newMachinePath, "Machine")
        Write-Host "  - Removed Docker paths from system PATH variable" -ForegroundColor Green
    }
}

Write-Host "`nDocker Desktop force uninstallation process completed!" -ForegroundColor Cyan
Write-Host "Note: A system restart is recommended to complete the cleanup process." -ForegroundColor Yellow
Write-Host "After restart, you can reinstall Docker Desktop if needed." -ForegroundColor Yellow
