BUSBUDDY DASHBOARD SETUP AND RUN GUIDE
===============================

SETUP INSTRUCTIONS
-----------------

1. First-time setup:
   - Make sure Node.js is installed: https://nodejs.org/ (LTS version recommended)
   - Ensure SQL Server is running with the BusBuddy database

2. Run the dashboard:
   - Double-click run-dashboard.cmd to install dependencies and start both:
     * The React frontend (http://localhost:3000)
     * The Express API backend (http://localhost:5000)

3. Manual setup (if needed):
   - Frontend:
     ```
     cd Forms/busbuddy-dashboard
     npm install
     npm start
     ```
   
   - Backend:
     ```
     cd Forms/busbuddy-dashboard/backend/api
     npm install
     node index.js
     ```

DEVELOPMENT NOTES
----------------

1. Project Structure:
   - src/App.tsx - Main application component
   - src/Dashboard.tsx - Dashboard component with data grid
   - backend/api/index.js - Express API connecting to SQL Server

2. API Endpoints:
   - GET /api/busroutes - Returns all bus routes from database

3. Configuration:
   - Backend configuration (connection strings) are in backend/api/.env
   - React app is configured with proxy to API server in package.json

TROUBLESHOOTING
--------------

1. If npm is not recognized:
   - Add Node.js to your PATH: $env:Path += ";C:\Program Files\nodejs"
   - Make it permanent: [Environment]::SetEnvironmentVariable("Path", $env:Path, [EnvironmentVariableTarget]::User)
   - Restart any open terminal windows

2. If API server cannot connect to database:
   - Check SQL Server connection settings in backend/api/.env
   - Ensure SQL Server is running and BusBuddy database exists
   - Verify user 'BusBuddyApp' has appropriate permissions
