@echo off
echo Building and running BusBuddy tests in Docker (simplified version)...

REM Build and run the tests
docker-compose -f docker-compose.tests.simple.yml up --build

echo.
echo Test completed.
echo.
