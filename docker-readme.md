# BusBuddy - Dockerized Application Setup

This guide provides comprehensive instructions for running and developing the BusBuddy application using Docker containers.

## Architecture

BusBuddy is containerized with three main components:

1. **SQL Server Database** - Persistent data store
2. **ASP.NET Core Application** - Backend API and application logic
3. **React Dashboard** - Frontend dashboard with Node.js API server

## Quick Start Guide

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) 
- At least 8GB of RAM for running all containers
- Git (for source code management)

### Production Setup

1. **Using Startup Scripts:**

   For Windows:
   ```
   docker-startup.cmd
   ```

   For macOS/Linux:
   ```bash
   ./docker-startup.sh
   ```

2. **Manual Setup:**

   ```bash
   docker-compose up --build -d
   ```

3. **Access the Applications:**
   - Main Application: http://localhost:5000
   - Dashboard: http://localhost:3000

### Development Setup

Run the development startup script and choose your environment:

```
dev-startup.cmd
```

Options:
1. Full stack (all components)
2. Backend only (database + ASP.NET Core)
3. Frontend only (database + dashboard)
4. Database only

## Container Details

### SQL Server
- **Container:** `busbuddy-sqlserver`
- **Ports:** 1433 (production), 14330 (development)
- **Credentials:**
  - Admin: `sa` / `BusB#ddy!2025`
  - App: `BusBuddyApp` / `App@P@ss!2025`
- **Database:** BusBuddy
- **Volume:** `busbuddy-sqlserver-data`

### ASP.NET Core Application
- **Container:** `busbuddy-webapp`
- **Ports:** 5000 (HTTP), 5001 (HTTPS)
- **Health Check:** http://localhost:5000/health
- **Environment Variables:**
  - `USE_DOCKER_DB`: Set to "true" in Docker
  - `CONNECTION_STRING`: Database connection string
  - `ASPNETCORE_ENVIRONMENT`: Production/Development

### React Dashboard
- **Container:** `busbuddy-dashboard`
- **Ports:**
  - 3000: React frontend
  - 5050: Dashboard API server (mapped from internal port 5000)
- **Environment Variables:**
  - `DB_SERVER`, `DB_USER`, `DB_PASSWORD`: Database connection
  - `NODE_ENV`: Production/Development
  - `REACT_APP_API_URL`: API endpoint URL

## Network Configuration

All containers communicate over the `busbuddy-network` bridge network:
- Database is accessible as `sqlserver`
- ASP.NET Core app is accessible as `webapp`
- Dashboard is accessible as `dashboard`

## Development Features

1. **Hot Reload:**
   - ASP.NET Core changes reload automatically with `dotnet watch`
   - React code changes reload with volume mounting

2. **Development Database:**
   - Additional test users and development settings
   - Port mapping allows direct connection from local tools

3. **Debugging:**
   - Source code mounted as volumes
   - Remote debugging ports exposed

## Troubleshooting

### Common Issues

1. **Container fails to start:**
   ```bash
   docker-compose logs <container_name>
   ```

2. **Database connection issues:**
   - Check if the SQL Server container is healthy:
     ```bash
     docker-compose ps sqlserver
     ```
   - Verify connection strings in environment variables

3. **Dashboard API connection issues:**
   - Verify CORS settings in WebStartup.cs
   - Check network connectivity between containers

### Recovery Steps

1. **Restart individual services:**
   ```bash
   docker-compose restart <service_name>
   ```

2. **Rebuild specific container:**
   ```bash
   docker-compose up -d --build <service_name>
   ```

3. **Complete reset (will delete all data!):**
   ```bash
   docker-compose down -v
   docker-compose up --build -d
   ```

## Advanced Configuration

### Custom Environment Variables

Create a `.env` file in the project root to override default settings:
```
SA_PASSWORD=YourCustomPassword
ASPNETCORE_ENVIRONMENT=Staging
```

### Custom SQL Initialization

Add custom SQL scripts to `docker/sql/` and they will run on container startup.

## Stopping the Application

To stop and preserve data:
```bash
docker-compose down
```

To stop and remove all data:
```bash
docker-compose down -v
```
