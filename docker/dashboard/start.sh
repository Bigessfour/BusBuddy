#!/bin/sh
# Log startup information
echo "Starting BusBuddy Dashboard containers..."

# Create necessary directories if they don't exist
mkdir -p /app/logs

# Start the API server
cd /app/api
echo "Starting dashboard API server on port ${PORT}..."
node index.js > /app/logs/api.log 2>&1 &
API_PID=$!

# Check if API server started successfully
sleep 3
if ! kill -0 $API_PID 2>/dev/null; then
  echo "Error: API server failed to start. Check logs at /app/logs/api.log"
  exit 1
fi
echo "API server started with PID: $API_PID"

# Serve the React app
cd /app
echo "Starting React frontend on port 3000..."
serve -s build -l 3000 > /app/logs/frontend.log 2>&1
