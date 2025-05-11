# GetFileStructure.ps1
# Script to output the project file structure

# Determine project root
$projectRoot = "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"
$outputFile = Join-Path $projectRoot "project_structure.txt"

Write-Host "Generating file structure report for BusBuddy project..." -ForegroundColor Cyan

# Create a string builder to collect all output
$output = New-Object System.Text.StringBuilder

# Create a nice header
[void]$output.AppendLine(@"
=============================================
BusBuddy Project Structure
=============================================
Generated: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
Root Path: $projectRoot
=============================================

"@)

# Function to build directory structure with indentation
function Get-DirectoryStructure {
    param (
        [Parameter(Mandatory = $true)]
        [string]$Path,
        
        [string]$Indent = "",
        
        [string]$ExcludePattern = "bin|obj|.vs|packages|node_modules",
        
        [System.Text.StringBuilder]$StringBuilder
    )

    # Get directories
    $dirs = Get-ChildItem -Path $Path -Directory | Where-Object { $_.Name -notmatch $ExcludePattern } | Sort-Object Name
    
    # Get files
    $files = Get-ChildItem -Path $Path -File | Sort-Object Name
    
    # Output files first
    foreach ($file in $files) {
        [void]$StringBuilder.AppendLine("$Indent|- $($file.Name)")
    }
    
    # Then handle directories
    foreach ($dir in $dirs) {
        [void]$StringBuilder.AppendLine("$Indent|+ $($dir.Name)/")
        Get-DirectoryStructure -Path $dir.FullName -Indent "$Indent|  " -ExcludePattern $ExcludePattern -StringBuilder $StringBuilder
    }
}

# Get the structure
Get-DirectoryStructure -Path $projectRoot -StringBuilder $output

# Add summary statistics
$totalFiles = (Get-ChildItem -Path $projectRoot -File -Recurse | Where-Object { $_.FullName -notmatch "bin|obj|.vs|packages|node_modules" }).Count
$totalDirs = (Get-ChildItem -Path $projectRoot -Directory -Recurse | Where-Object { $_.FullName -notmatch "bin|obj|.vs|packages|node_modules" }).Count
$scriptFiles = (Get-ChildItem -Path $projectRoot -File -Recurse -Filter "*.ps1").Count
$markdownFiles = (Get-ChildItem -Path $projectRoot -File -Recurse -Filter "*.md").Count
$csharpFiles = (Get-ChildItem -Path $projectRoot -File -Recurse -Filter "*.cs").Count

[void]$output.AppendLine(@"

=============================================
Summary Statistics
=============================================
Total Files: $totalFiles
Total Directories: $totalDirs
PowerShell Scripts: $scriptFiles
Markdown Files: $markdownFiles
C# Files: $csharpFiles
=============================================
"@)

# Write to file all at once
try {
    Set-Content -Path $outputFile -Value $output.ToString() -Force
    Write-Host "File structure report generated at: $outputFile" -ForegroundColor Green
    Write-Host "You can open this file to view your project structure." -ForegroundColor Green
    
    # Display the content in the console as well
    Write-Host "`nProject Structure Preview:" -ForegroundColor Yellow
    Get-Content -Path $outputFile | Select-Object -First 30
    Write-Host "... (more content in the file)" -ForegroundColor Yellow
}
catch {
    Write-Host "Error writing to file: $_" -ForegroundColor Red
    Write-Host "Displaying output to console instead:" -ForegroundColor Yellow
    Write-Host $output.ToString()
}
