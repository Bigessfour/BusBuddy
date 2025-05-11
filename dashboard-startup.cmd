@echo off
REM Dashboard Docker Startup Script for BusBuddy

echo BusBuddy Dashboard Development Environment
echo =========================================
echo.

REM Check if Docker is running
docker info >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Docker is not running. Please start Docker Desktop and try again.
    exit /b 1
)

REM Parse command line arguments
set REBUILD=0
for %%a in (%*) do (
    if "%%a"=="--rebuild" set REBUILD=1
)

REM Stop any existing dashboard containers
echo Stopping any existing BusBuddy Dashboard containers...
docker-compose -f docker-compose.dashboard.yml down

REM Clean up if rebuild requested
if %REBUILD%==1 (
    echo Removing old container images...
    docker rmi busbuddy-blazor-dashboard -f 2>nul
)

REM Build and start containers
echo Building and starting BusBuddy Dashboard...
docker-compose -f docker-compose.dashboard.yml up --build -d

echo.
echo BusBuddy Dashboard is now running!
echo.
echo - Modern Dashboard URL: http://localhost:5500/modern-dashboard
echo - Classic Dashboard URL: http://localhost:5500/dashboard
echo - Database: localhost,1433 (SQL Server)
echo   Username: BusBuddyApp
echo   Password: App@P@ss!2025
echo.
echo For logs, use: docker-compose -f docker-compose.dashboard.yml logs -f
echo To stop, use: docker-compose -f docker-compose.dashboard.yml down
echo.
