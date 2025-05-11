# ExecuteOrganization.ps1
# Script to organize .ps1 and .md files into appropriate folders

# Determine project root
$projectRoot = "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"
if (-not (Test-Path (Join-Path $projectRoot "BusBuddy.csproj"))) {
    Write-Host "Error: Unable to find project root directory." -ForegroundColor Red
    Write-Host "Please run this script from the project root or Scripts directory." -ForegroundColor Red
    exit 1
}

# Ensure directories exist
$scriptsFolder = Join-Path $projectRoot "Scripts"
$docsFolder = Join-Path $projectRoot "Docs"

if (-not (Test-Path $scriptsFolder)) {
    New-Item -Path $scriptsFolder -ItemType Directory
    Write-Host "Created Scripts folder" -ForegroundColor Green
}

if (-not (Test-Path $docsFolder)) {
    New-Item -Path $docsFolder -ItemType Directory
    Write-Host "Created Docs folder" -ForegroundColor Green
}

# Move PowerShell scripts
Write-Host "Moving PowerShell scripts to Scripts directory..." -ForegroundColor Cyan
Get-ChildItem -Path $projectRoot -Filter "*.ps1" -Recurse | Where-Object { 
    $_.DirectoryName -ne $scriptsFolder 
} | ForEach-Object {
    Write-Host "Moving $($_.Name) to Scripts directory"
    Move-Item -Path $_.FullName -Destination $scriptsFolder -Force
}

# Move Markdown files
Write-Host "Moving Markdown files to Docs directory..." -ForegroundColor Cyan
Get-ChildItem -Path $projectRoot -Filter "*.md" -Recurse | Where-Object { 
    $_.DirectoryName -ne $docsFolder 
} | ForEach-Object {
    Write-Host "Moving $($_.Name) to Docs directory"
    Move-Item -Path $_.FullName -Destination $docsFolder -Force
}

Write-Host "Organization complete!" -ForegroundColor Green
Write-Host "Files have been organized according to the file organization strategy." -ForegroundColor Green
Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
