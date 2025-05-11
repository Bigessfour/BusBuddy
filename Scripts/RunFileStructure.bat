@echo off
title BusBuddy File Structure Generator
color 0A

echo =============================================
echo BusBuddy File Structure Generator
echo =============================================
echo.

REM Check if PowerShell is available
where powershell >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: PowerShell is not available on this system.
    echo Please install PowerShell or check your PATH settings.
    goto :error
)

REM Check if script exists
if not exist "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\GetFileStructure.ps1" (
    echo ERROR: Cannot find the GetFileStructure.ps1 script.
    echo Expected location: c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\GetFileStructure.ps1
    goto :error
)

echo Running BusBuddy file structure generator...
echo This may take a moment...
echo.

REM Run the PowerShell script with detailed output
powershell -NoProfile -ExecutionPolicy Bypass -Command "& {$VerbosePreference = 'Continue'; Write-Host 'Starting PowerShell execution...'; try { & 'c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\GetFileStructure.ps1' } catch { Write-Host ('Error: ' + $_.Exception.Message) -ForegroundColor Red; exit 1 }}"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: The PowerShell script encountered an error.
    goto :error
)

echo.
echo If successful, the file is located at: c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\project_structure.txt
echo.
echo You can now open the file to view your project structure.
goto :end

:error
echo.
echo Troubleshooting tips:
echo 1. Make sure you're running this as an administrator
echo 2. Check if the PowerShell script exists at the specified path
echo 3. Try running the PowerShell script directly in PowerShell
echo.

:end
echo.
echo Press any key to exit...
pause >nul
