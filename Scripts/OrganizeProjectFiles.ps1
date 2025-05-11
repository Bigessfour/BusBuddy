# OrganizeProjectFiles.ps1
# Script to move .ps1 files to Scripts folder and .md files to Docs folder

$projectRoot = "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"
$scriptsFolder = Join-Path $projectRoot "Scripts"
$docsFolder = Join-Path $projectRoot "Docs"

# Ensure folders exist
if (-not (Test-Path $scriptsFolder)) {
    New-Item -Path $scriptsFolder -ItemType Directory
    Write-Host "Created Scripts folder"
}

if (-not (Test-Path $docsFolder)) {
    New-Item -Path $docsFolder -ItemType Directory
    Write-Host "Created Docs folder"
}

# Find and move PowerShell scripts (.ps1) that aren't already in the Scripts folder
Get-ChildItem -Path $projectRoot -Filter "*.ps1" -Recurse | Where-Object { 
    $_.DirectoryName -notlike "*\Scripts*" -and 
    $_.DirectoryName -notlike "*\Tests*" -and
    $_.Name -ne "OrganizeProjectFiles.ps1" # Don't move this script while it's running
} | ForEach-Object {
    $destination = Join-Path $scriptsFolder $_.Name
    Write-Host "Moving $($_.FullName) to $destination"
    Move-Item -Path $_.FullName -Destination $destination -Force
}

# Find and move Markdown files (.md) that aren't already in the Docs folder
Get-ChildItem -Path $projectRoot -Filter "*.md" -Recurse | Where-Object { 
    $_.DirectoryName -notlike "*\Docs*" -and 
    $_.DirectoryName -notlike "*\Tests*" 
} | ForEach-Object {
    $destination = Join-Path $docsFolder $_.Name
    Write-Host "Moving $($_.FullName) to $destination"
    Move-Item -Path $_.FullName -Destination $destination -Force
}

Write-Host "File organization complete."
