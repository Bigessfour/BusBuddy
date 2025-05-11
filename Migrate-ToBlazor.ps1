# Migrate-ToBlazor.ps1
# Helper script to migrate BusBuddy from Windows Forms to Blazor
param (
    [switch]$Clean,        # Clean build artifacts
    [switch]$NoBuild,      # Skip building
    [switch]$RunDocker     # Run in Docker after migration
)

Write-Host "BusBuddy - Windows Forms to Blazor Migration Helper" -ForegroundColor Cyan
Write-Host "--------------------------------------------------" -ForegroundColor Cyan

# Current location
$currentDir = Get-Location

# Step 1: Check if all required files exist
Write-Host "Step 1: Checking required files..." -ForegroundColor Green

$requiredFiles = @(
    "Program.cs",
    "WebStartup.cs",
    "Dockerfile",
    "docker-compose.yml",
    "Pages/Dashboard.razor"
)

$missingFiles = @()
foreach ($file in $requiredFiles) {
    if (-not (Test-Path $file)) {
        $missingFiles += $file
    }
}

if ($missingFiles.Count -gt 0) {
    Write-Host "Error: The following required files are missing:" -ForegroundColor Red
    foreach ($file in $missingFiles) {
        Write-Host "  - $file" -ForegroundColor Red
    }
    exit 1
}

# Step 2: Clean build artifacts if requested
if ($Clean) {
    Write-Host "Step 2: Cleaning build artifacts..." -ForegroundColor Green
    
    # Clean bin and obj directories
    if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
    if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }
    
    Write-Host "  Build artifacts cleaned." -ForegroundColor Green
}
else {
    Write-Host "Step 2: Skipping clean (use -Clean to clean build artifacts)" -ForegroundColor Yellow
}

# Step 3: Check for Windows Forms references that need to be removed
Write-Host "Step 3: Checking for Windows Forms references..." -ForegroundColor Green

$csprojContent = Get-Content "BusBuddy.csproj" -Raw
$hasWinForms = $csprojContent -match "UseWindowsForms"

if ($hasWinForms) {
    Write-Host "  Windows Forms references found in BusBuddy.csproj." -ForegroundColor Yellow
    Write-Host "  Consider removing the following from the project file:" -ForegroundColor Yellow
    Write-Host "  - <UseWindowsForms>true</UseWindowsForms>" -ForegroundColor Yellow
    Write-Host "  - <PackageReference Include=\"System.Windows.Forms\" ... />" -ForegroundColor Yellow
}
else {
    Write-Host "  No Windows Forms references found in project file." -ForegroundColor Green
}

# Step 4: Build the application
if (-not $NoBuild) {
    Write-Host "Step 4: Building the application..." -ForegroundColor Green
    
    dotnet restore
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: Failed to restore packages." -ForegroundColor Red
        exit 1
    }
    
    dotnet build
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error: Failed to build application." -ForegroundColor Red
        exit 1
    }
    
    Write-Host "  Application built successfully." -ForegroundColor Green
}
else {
    Write-Host "Step 4: Skipping build (use -NoBuild to skip building)" -ForegroundColor Yellow
}

# Step 5: Run tests
Write-Host "Step 5: Running Blazor-specific tests..." -ForegroundColor Green

dotnet test --filter "FullyQualifiedName~DashboardBlazorTests"
if ($LASTEXITCODE -ne 0) {
    Write-Host "  Warning: Some Blazor tests failed. Review the test output above." -ForegroundColor Yellow
}
else {
    Write-Host "  Blazor tests passed successfully." -ForegroundColor Green
}

# Step 6: Run in Docker if requested
if ($RunDocker) {
    Write-Host "Step 6: Running the application in Docker..." -ForegroundColor Green
    
    Write-Host "  Stopping any existing containers..." -ForegroundColor Yellow
    docker-compose down
    
    Write-Host "  Building and starting containers..." -ForegroundColor Yellow
    docker-compose up --build -d
    
    Write-Host "  Container status:" -ForegroundColor Cyan
    docker-compose ps
    
    # Display access information
    Write-Host "`nBusBuddy is now running in Docker!" -ForegroundColor Green
    Write-Host "Access the application at: http://localhost:5000" -ForegroundColor Yellow
    Write-Host "To view logs: docker-compose logs -f" -ForegroundColor Yellow
    Write-Host "To stop: docker-compose down" -ForegroundColor Yellow
}
else {
    Write-Host "Step 6: Skipping Docker deployment (use -RunDocker to deploy)" -ForegroundColor Yellow
    
    # Display run instructions
    Write-Host "`nTo run the application locally:" -ForegroundColor Green
    Write-Host "dotnet run" -ForegroundColor Yellow
    Write-Host "Then access the application at: https://localhost:5001" -ForegroundColor Yellow
    
    Write-Host "`nTo run in Docker later:" -ForegroundColor Green
    Write-Host "docker-compose up --build -d" -ForegroundColor Yellow
}

Write-Host "`nMigration process completed!" -ForegroundColor Cyan
