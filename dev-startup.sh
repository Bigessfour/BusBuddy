#!/bin/bash
# Development startup script for BusBuddy in Docker

echo "Starting BusBuddy in development mode..."

# Set development environment variables
export ASPNETCORE_ENVIRONMENT=Development
export DOTNET_ENVIRONMENT=Development

echo
echo "Available development options:"
echo "1. Full stack (database + backend + dashboard)"
echo "2. Backend only (database + ASP.NET Core)"
echo "3. Frontend only (database + dashboard)"
echo "4. Database only"
echo

read -p "Select an option (1-4): " choice

case $choice in
  1)
    echo "Starting full stack development environment..."
    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build
    ;;
  2)
    echo "Starting backend development environment..."
    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver webapp
    ;;
  3)
    echo "Starting frontend development environment..."
    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver dashboard
    ;;
  4)
    echo "Starting database only..."
    docker-compose -f docker-compose.yml -f docker-compose.dev.yml up --build sqlserver
    ;;
  *)
    echo "Invalid option. Exiting."
    exit 1
    ;;
esac

echo "Done"
