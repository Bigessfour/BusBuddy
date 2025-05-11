# OrganizeFiles.ps1
# Script to organize PowerShell and Markdown files into appropriate folders

# Function to check if a command is available
function Test-CommandExists {
    param ($command)
    
    $oldPreference = $ErrorActionPreference
    $ErrorActionPreference = 'stop'
    
    try {
        if (Get-Command $command -ErrorAction Stop) {
            return $true
        }
    }
    catch {
        return $false
    }
    finally {
        $ErrorActionPreference = $oldPreference
    }
}

# Check for required tools
Write-Host "Checking for required tools..." -ForegroundColor Cyan
$missingTools = @()

# Check for dotnet
if (-not (Test-CommandExists "dotnet")) {
    $missingTools += "dotnet"
}

# Add more tool checks as needed
# if (-not (Test-CommandExists "some-other-tool")) {
#     $missingTools += "some-other-tool"
# }

# If any tools are missing, print instructions
if ($missingTools.Count -gt 0) {
    Write-Host "`nWARNING: The following required tools are not installed or not in your PATH:" -ForegroundColor Red
    foreach ($tool in $missingTools) {
        Write-Host "  - $tool" -ForegroundColor Red
    }
    
    Write-Host "`nInstallation instructions:" -ForegroundColor Yellow
    if ($missingTools -contains "dotnet") {
        Write-Host "  - dotnet: Download and install the .NET SDK from https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    }
    
    $proceed = Read-Host "`nDo you want to continue anyway? (y/n)"
    if ($proceed -ne "y") {
        Write-Host "Script execution cancelled." -ForegroundColor Red
        exit
    }
    
    Write-Host "Continuing script execution despite missing tools..." -ForegroundColor Yellow
}

# Set project root directory
$projectRoot = "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"
$scriptsFolder = Join-Path $projectRoot "Scripts"
$docsFolder = Join-Path $projectRoot "Docs"

Write-Host "Starting file organization process..." -ForegroundColor Cyan

# Ensure the directories exist
if (-not (Test-Path $scriptsFolder)) {
    New-Item -Path $scriptsFolder -ItemType Directory -Force | Out-Null
    Write-Host "Created Scripts folder" -ForegroundColor Green
}

if (-not (Test-Path $docsFolder)) {
    New-Item -Path $docsFolder -ItemType Directory -Force | Out-Null
    Write-Host "Created Docs folder" -ForegroundColor Green
}

# Find all .ps1 files that aren't already in the Scripts folder
$psFiles = Get-ChildItem -Path $projectRoot -Filter "*.ps1" -Recurse | 
    Where-Object { 
        $_.DirectoryName -ne $scriptsFolder -and 
        $_.DirectoryName -notlike "$scriptsFolder\*" -and
        $_.DirectoryName -notlike "$projectRoot\bin\*" -and
        $_.DirectoryName -notlike "$projectRoot\obj\*" -and
        $_.DirectoryName -notlike "$projectRoot\.vs\*"
    }

# Move PowerShell files
if ($psFiles.Count -gt 0) {
    Write-Host "`nMoving $($psFiles.Count) PowerShell files to the Scripts folder:" -ForegroundColor Yellow
    foreach ($file in $psFiles) {
        $destinationPath = Join-Path $scriptsFolder $file.Name
        
        # Check if file already exists in destination
        if (Test-Path $destinationPath) {
            $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
            $extension = [System.IO.Path]::GetExtension($file.Name)
            $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
            $destinationPath = Join-Path $scriptsFolder "$baseName`_$timestamp$extension"
        }
        
        Write-Host "  Moving $($file.FullName) to $destinationPath" -ForegroundColor White
        Move-Item -Path $file.FullName -Destination $destinationPath -Force
    }
}
else {
    Write-Host "`nNo PowerShell files (.ps1) found outside the Scripts directory." -ForegroundColor Green
}

# Find all .md files that aren't already in the Docs folder
$mdFiles = Get-ChildItem -Path $projectRoot -Filter "*.md" -Recurse | 
    Where-Object { 
        $_.DirectoryName -ne $docsFolder -and 
        $_.DirectoryName -notlike "$docsFolder\*" -and
        $_.DirectoryName -notlike "$projectRoot\bin\*" -and
        $_.DirectoryName -notlike "$projectRoot\obj\*" -and
        $_.DirectoryName -notlike "$projectRoot\.vs\*"
    }

# Move Markdown files
if ($mdFiles.Count -gt 0) {
    Write-Host "`nMoving $($mdFiles.Count) Markdown files to the Docs folder:" -ForegroundColor Yellow
    foreach ($file in $mdFiles) {
        $destinationPath = Join-Path $docsFolder $file.Name
        
        # Check if file already exists in destination
        if (Test-Path $destinationPath) {
            $baseName = [System.IO.Path]::GetFileNameWithoutExtension($file.Name)
            $extension = [System.IO.Path]::GetExtension($file.Name)
            $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
            $destinationPath = Join-Path $docsFolder "$baseName`_$timestamp$extension"
        }
        
        Write-Host "  Moving $($file.FullName) to $destinationPath" -ForegroundColor White
        Move-Item -Path $file.FullName -Destination $destinationPath -Force
    }
}
else {
    Write-Host "`nNo Markdown files (.md) found outside the Docs directory." -ForegroundColor Green
}

# Final summary
Write-Host "`nFile organization complete!" -ForegroundColor Cyan
Write-Host "- PowerShell scripts (.ps1) are now in: $scriptsFolder" -ForegroundColor White
Write-Host "- Documentation files (.md) are now in: $docsFolder" -ForegroundColor White
Write-Host "`nYou may want to update your project references if any moved files are referenced in your code." -ForegroundColor Yellow
