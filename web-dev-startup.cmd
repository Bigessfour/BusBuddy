@echo off
REM Web Development Docker Startup Script for BusBuddy

echo BusBuddy Web Development Environment Setup
echo =========================================
echo.
echo This script runs only the web components of BusBuddy in Docker
echo while leaving the Windows Forms app to run locally.
echo.

REM Check if Docker is running
docker info >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Docker is not running. Please start Docker Desktop and try again.
    exit /b 1
)

REM Stop any existing containers
echo Stopping any existing BusBuddy containers...
docker-compose -f docker-compose.web.yml down

REM Build and start containers
echo Building and starting web development environment...
docker-compose -f docker-compose.web.yml up --build -d

echo.
echo BusBuddy web development environment is now running!
echo.
echo - React Dashboard: http://localhost:3000
echo - Database: localhost,1433 (SQL Server)
echo   Username: BusBuddyApp
echo   Password: App@P@ss!2025
echo.
echo You can now run the Windows Forms application locally with:
echo   dotnet run
echo.
echo To view Docker logs:
echo   docker-compose -f docker-compose.web.yml logs -f
echo.
echo To stop the containers:
echo   docker-compose -f docker-compose.web.yml down
echo.
