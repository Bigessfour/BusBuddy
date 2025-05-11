# BusBuddy Rebuild Plan

## Current Approach

We are rebuilding the BusBuddy application from the ground up using the following approach:

1. Starting with core files in the current workspace
2. Gradually adding files from the old BusBuddyMVP project (which is not in this workspace)
3. Building incrementally, ensuring a clean build at each step
4. Only referencing existing files in the workspace
5. Using the README to understand the future vision of the project
6. Using GitHub for additional assistance

## Current State

As of May 9, 2025, the project has the following structure:

- **Program.cs**: Entry point that initializes Serilog, sets up DI, and launches the Dashboard form
- **Data/DatabaseHelper.cs**: SQLite-based helper class for database operations
- **Data/BusBuddyContext.cs**: Entity Framework Core DbContext for SQL Server
- **Forms/Dashboard.cs**: Main form that will be the central viewing area for all functions
- **Forms/Dashboard.Designer.cs**: Auto-generated designer code for the Dashboard form
- **Forms/RouteManagementForm.cs**: Form for managing bus routes

## Database Discrepancy

There is currently a discrepancy in the database approach:
1. The README and BusBuddyContext.cs indicate using SQL Server with EF Core
2. The DatabaseHelper.cs implements SQLite with Dapper

This will need to be resolved as we progress with the rebuild.

## Missing Components

The following components are referenced but not yet implemented:
1. Models folder with entity classes (referenced in BusBuddyContext.cs)
2. Interfaces folder with IDatabaseHelper interface
3. Complete implementation of Dashboard.cs (methods referenced but not implemented)
4. MaterialSkin integration (referenced in README)

## Next Steps

1. Create Models folder with basic entity classes
2. Implement required methods in Dashboard.cs
3. Decide on database approach (SQL Server vs SQLite)
4. Add MaterialSkin integration
5. Gradually add additional forms as needed

## Guidelines

- Only implement functionality that is explicitly provided or requested
- Don't assume implementations from the old project without seeing the code
- Maintain clean builds at each step
- Follow the project structure and patterns established in the current files
- Use MaterialSkin.2 for UI components as mentioned in README
- Use proper logging with Serilog as implemented in Program.cs
