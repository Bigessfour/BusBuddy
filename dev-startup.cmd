@echo off
REM Development startup script for BusBuddy in Docker

echo Starting BusBuddy in development mode...

REM Set development environment variables
set ASPNETCORE_ENVIRONMENT=Development
set DOTNET_ENVIRONMENT=Development

echo:
echo Available development options:
echo 1. Full stack (database + backend + dashboard)
echo 2. Backend only (database + ASP.NET Core)
echo 3. Frontend only (database + dashboard)
echo 4. Database only
echo:

choice /C 1234 /M "Select an option (1-4):"

if errorlevel 4 goto :database
if errorlevel 3 goto :frontend
if errorlevel 2 goto :backend
if errorlevel 1 goto :fullstack

:fullstack
echo Starting full stack development environment...
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build
goto :end

:backend
echo Starting backend development environment...
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver webapp
goto :end

:frontend
echo Starting frontend development environment...
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver dashboard
goto :end

:database
echo Starting database only...
docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver
goto :end

:end
echo Done
