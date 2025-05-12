# Fix-BuildIssues.ps1
# This script addresses common build issues in the BusBuddy project

Write-Host "Fixing build issues for BusBuddy Blazor..." -ForegroundColor Cyan

# 1. Clean the solution
Write-Host "Cleaning solution..." -ForegroundColor Yellow
dotnet clean

# 2. Restore packages explicitly
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
dotnet restore

# 3. Update package versions that may be causing issues
Write-Host "Updating problematic package references..." -ForegroundColor Yellow

# Get the content of Directory.Packages.props
$packagesProps = Get-Content -Path "Directory.Packages.props" -Raw

# Add explicit namespace imports to ensure InMemoryDatabase is available
Write-Host "Adding explicit imports for EntityFrameworkCore.InMemory..." -ForegroundColor Yellow
foreach ($testFile in Get-ChildItem -Path "Tests" -Filter "*.cs") {
    $content = Get-Content -Path $testFile.FullName -Raw
    
    # Only add the import if it's not already present
    if (-not $content.Contains("using Microsoft.EntityFrameworkCore.InMemory;")) {
        $content = $content -replace "using Microsoft.EntityFrameworkCore;", "using Microsoft.EntityFrameworkCore;`r`nusing Microsoft.EntityFrameworkCore.InMemory;"
        Set-Content -Path $testFile.FullName -Value $content
    }
}

# 4. Create a temporary workaround for UseInMemoryDatabase if needed
Write-Host "Creating workaround for InMemory database access..." -ForegroundColor Yellow
$helperContent = @"
using Microsoft.EntityFrameworkCore;

namespace BusBuddy.Tests
{
    public static class InMemoryDatabaseHelper
    {
        public static DbContextOptionsBuilder<T> UseInMemoryDatabaseSafe<T>(this DbContextOptionsBuilder<T> builder, string name) where T : DbContext
        {
            return builder.UseInMemoryDatabase(name);
        }
    }
}
"@

Set-Content -Path "Tests\InMemoryDatabaseHelper.cs" -Value $helperContent

# 5. Build the main project first
Write-Host "Building main project..." -ForegroundColor Yellow
dotnet build BusBuddy.csproj

# 6. Build the test project
Write-Host "Building test project..." -ForegroundColor Yellow
dotnet build Tests\BusBuddy.Tests.csproj

Write-Host "Build fix process completed. Check for any remaining errors." -ForegroundColor Cyan
