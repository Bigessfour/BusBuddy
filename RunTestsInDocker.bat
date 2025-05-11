@echo off
echo Building and running BusBuddy tests in Docker...

docker-compose -f docker-compose.tests.yml up --build

echo.
echo Test completed.
echo.
