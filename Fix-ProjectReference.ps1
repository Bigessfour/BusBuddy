# Fix-ProjectReference.ps1
# Quick script to fix the project reference issue that's causing the 419 errors

Write-Host "Fixing project reference issues (root cause of 419 errors)..." -ForegroundColor Cyan

# Navigate to project folder
cd "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy"

# Create a temporary solution file to ensure proper project relationships
$solutionContent = @"
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.3.32922.545
MinimumVisualStudioVersion = 10.0.40219.1
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "BusBuddy", "BusBuddy.csproj", "{583C2B2D-F9CA-4C03-9D9A-89C590234D73}"
EndProject
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "BusBuddy.Tests", "Tests\BusBuddy.Tests.csproj", "{155BB97D-ED3D-409C-840A-7F7D0362625C}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{583C2B2D-F9CA-4C03-9D9A-89C590234D73}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{583C2B2D-F9CA-4C03-9D9A-89C590234D73}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{583C2B2D-F9CA-4C03-9D9A-89C590234D73}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{583C2B2D-F9CA-4C03-9D9A-89C590234D73}.Release|Any CPU.Build.0 = Release|Any CPU
		{155BB97D-ED3D-409C-840A-7F7D0362625C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{155BB97D-ED3D-409C-840A-7F7D0362625C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{155BB97D-ED3D-409C-840A-7F7D0362625C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{155BB97D-ED3D-409C-840A-7F7D0362625C}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
EndGlobal
"@

Set-Content -Path "BusBuddy.sln.fixed" -Value $solutionContent
Write-Host "Created a properly configured solution file: BusBuddy.sln.fixed" -ForegroundColor Green

# Now run the build using this solution to properly establish project relationships
Write-Host "Running restore with fixed solution..." -ForegroundColor Yellow
dotnet restore "BusBuddy.sln.fixed"

# Then build just the test project with proper references
Write-Host "Building test project with correct references..." -ForegroundColor Yellow
dotnet build "Tests\BusBuddy.Tests.csproj" -c Debug

Write-Host @"

IMPORTANT: If the build succeeds, replace your current solution file:
Copy-Item "BusBuddy.sln.fixed" "BusBuddy.sln" -Force

This will ensure proper project structure in the solution.
"@ -ForegroundColor Cyan
