#!/bin/bash
# Docker Compose startup script for BusBuddy application

echo "Starting BusBuddy application with Docker Compose..."

# Stop and remove any existing containers
echo "Stopping any existing containers..."
docker-compose down

# Build and start the containers
echo "Building and starting containers..."
docker-compose up --build -d

# Wait for services to initialize
echo "Waiting for services to initialize..."
sleep 10

# Display container status
echo "Container status:"
docker-compose ps

echo ""
echo "BusBuddy application is now running!"
echo "- Access the ASP.NET Core app at: http://localhost:5000"
echo "- Access the React dashboard at: http://localhost:3000"
echo ""
echo "To view logs, use: docker-compose logs -f"
echo "To stop the application, use: docker-compose down"
