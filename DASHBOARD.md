# BusBuddy Dashboard Quick Reference

## Overview
The BusBuddy Dashboard provides a real-time view of bus operations, including active routes, buses, drivers, and alerts. The dashboard is accessible at `https://localhost:5001/dashboard`.

## Features
- **Metrics Overview**: Shows counts of active buses, routes, drivers, and alerts
- **Route Status Chart**: Visual representation of route statuses (OnTime, Delayed, Cancelled)
- **Active Alerts**: Table of current alerts with severity indicators

## Access Methods
1. **Direct URL**: Navigate to `https://localhost:5001/dashboard` in your browser
2. **Launch Script**: Run `LaunchDashboard.ps1` to open both the application and dashboard

## Troubleshooting
- If the dashboard doesn't appear, ensure the application is running
- Check if port 5001 is already in use by another application
- Look at the logs in the `logs` directory for any errors

## Technical Details
- Built with Blazor Server and ASP.NET Core
- Data refreshes automatically every 30 seconds
- Uses Chart.js for visualizations

## Development
The dashboard is part of the main BusBuddy application. Key files:
- `Pages/Dashboard.razor`: Main Blazor component
- `Services/Dashboard/DashboardService.cs`: Data access service
- `Controllers/DashboardController.cs`: API endpoints
- `wwwroot/js/dashboard.js`: JavaScript for Chart.js
