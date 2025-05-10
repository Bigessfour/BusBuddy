# Script to fix nullability warnings in test files

# Path to test files
$routeTestFile = ".\Tests\RouteManagementForm_LoadingTests.cs"
$vehicleTestFile = ".\Tests\VehiclesManagementForm_DataDisplayTests.cs"

# Create backup of original files
Copy-Item $routeTestFile "$routeTestFile.bak" -Force
Copy-Item $vehicleTestFile "$vehicleTestFile.bak" -Force

# Read file content
$routeContent = Get-Content $routeTestFile -Raw
$vehicleContent = Get-Content $vehicleTestFile -Raw

# Replace non-nullable Exception with nullable Exception? in the files
$routeContent = $routeContent -replace 'It\.IsAny<Exception>\(\)', 'It.IsAny<Exception?>()'
$routeContent = $routeContent -replace 'It\.Is<Exception>\(', 'It.Is<Exception?>(';
$routeContent = $routeContent -replace 'It\.Is<Func<It\.IsAnyType, Exception, string>>', 'It.Is<Func<It.IsAnyType, Exception?, string>>';

$vehicleContent = $vehicleContent -replace 'It\.IsAny<Exception>\(\)', 'It.IsAny<Exception?>()'
$vehicleContent = $vehicleContent -replace 'It\.Is<Exception>\(', 'It.Is<Exception?>(';
$vehicleContent = $vehicleContent -replace 'It\.Is<Func<It\.IsAnyType, Exception, string>>', 'It.Is<Func<It.IsAnyType, Exception?, string>>';

# Write modified content back to files
Set-Content $routeTestFile $routeContent -Force
Set-Content $vehicleTestFile $vehicleContent -Force

Write-Output "Files updated successfully."
