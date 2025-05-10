# BusBuddy Dashboard

A modern React + Material-UI dashboard for BusBuddy, displaying SQL Server data with a professional UI.

## Features
- Responsive, sortable, filterable grid of bus routes
- Dark/light mode toggle
- Column visibility customization (saved to localStorage)
- Refresh button, loading spinner, and error handling
- Backend API (Node.js/Express) connects to SQL Server

## Prerequisites
- Node.js & npm (https://nodejs.org/)
- SQL Server running with BusBuddy database and BusRoutes table

## Setup Instructions

### 1. Install dependencies
```
cd Forms/busbuddy-dashboard
npm install
npm install @mui/material @mui/icons-material @mui/x-data-grid axios
```

### 2. Backend API Setup
```
cd backend/api
npm init -y
npm install express mssql cors dotenv
```

Create a `.env` file in `backend/api` (optional, for custom config):
```
DB_USER=BusBuddyApp
DB_PASS=App@P@ss!2025
DB_HOST=localhost
DB_NAME=BusBuddy
DB_PORT=1433
PORT=5000
```

Start the backend API:
```
node index.js
```

### 3. Start the React App
In a new terminal:
```
cd Forms/busbuddy-dashboard
npm start
```

### 4. Proxy Setup (Optional)
Add this to `package.json` in `busbuddy-dashboard` for local API proxy:
```
  "proxy": "http://localhost:5000",
```

## File Structure
- `src/Dashboard.tsx` — Main dashboard component
- `src/App.tsx` — App entry point
- `backend/api/index.js` — Express API for SQL Server

## Notes
- Make sure SQL Server is running and accessible.
- The dashboard expects the API at `/api/busroutes`.
- Customize columns and theme as needed.

---

For questions or issues, see the main BusBuddy documentation or contact your project maintainer.
