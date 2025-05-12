@echo off
REM Docker Compose startup script for BusBuddy application

echo Starting BusBuddy application with Docker Compose...

REM Stop and remove any existing containers
echo Stopping any existing containers...
docker-compose down

REM Build and start the containers
echo Building and starting containers...
docker-compose up --build -d

REM Wait for services to initialize
echo Waiting for services to initialize...
timeout /t 10 /nobreak > nul

REM Display container status
echo Container status:
docker-compose ps

echo.
echo BusBuddy application is now running!
echo - Access the BusBuddy Dashboard at: http://localhost:5000/Dashboard
echo.
echo To view logs, use: docker-compose logs -f
echo To stop the application, use: docker-compose down
