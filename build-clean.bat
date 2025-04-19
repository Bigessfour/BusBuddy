@echo off
echo Building BusBuddy with clean output (no warnings)...
dotnet build --nologo /clp:ErrorsOnly
if %ERRORLEVEL% EQU 0 (
  echo Build successful!
) else (
  echo Build failed with errors.
)