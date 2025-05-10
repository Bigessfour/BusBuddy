# BusBuddy - School Bus Management System

![CI](https://github.com/Bigessfour/BusBuddy/actions/workflows/ci.yml/badge.svg)

A .NET 8.0 Windows Forms application for managing school bus operations, built with SQL Server Express, Entity Framework Core, MaterialSkin.2 for UI, and Microsoft.Extensions.Logging for diagnostics.

## Current Status (as of May 12, 2025)
- **Build Status**: [![CI](https://github.com/Bigessfour/BusBuddy/actions/workflows/ci.yml/badge.svg)](https://github.com/Bigessfour/BusBuddy/actions/workflows/ci.yml)
- **Version**: 1.0.0-beta
- **Errors**: 0 (resolved CS1061 in `BusBuddyContext.cs`, WFO1000 in `RouteEditorDialog.cs`)
- **Warnings**: 0 (resolved NU1603 for EF Core packages)
- **Current Floor**: 38 (stable base, database initialized)
- **Progress**: Database rebuilt and initialized, application running, implementing organized branch strategy
- **New Features**:
  - Added license expiration alerts to Driver Management form
  - Improved driver management with enhanced UI
  - Added comprehensive test suite for form validation

## Project Health
- **Build Status**: 0 errors, 0 warnings
- **Test Coverage**: 25.73% (added comprehensive tests for Route and Vehicle Management forms)
- **Architectural Health**: Resolved all 22 architecture violations (added [Required] attributes to entity models)
- **Log Errors**: No recent errors

## Project Structure
```
BusBuddy/
├── Data/                 # Database and data access
│   ├── BusBuddyContext.cs
│   ├── DatabaseHelper.cs
│   └── Interfaces/
│       └── IDatabaseHelper.cs
├── Forms/                # UI Windows Forms
│   ├── Dashboard.cs
│   ├── DeleteDriverDialog.cs
│   ├── DriverEditorDialog.cs
│   ├── DriverManagementForm.cs
│   ├── RouteEditorDialog.cs
│   └── VehiclesManagementForm.cs
├── Models/               # Domain models
│   ├── Entities/         # Database entity models
│   │   ├── Driver.cs
│   │   ├── Route.cs
│   │   └── Vehicle.cs
│   └── ValueObjects/     # Value objects for entity properties
│       └── Address.cs
└── Tests/               # Test project
    ├── DeleteDriverTests.cs
    ├── DriverTests.cs
    ├── RouteManagementFormTests.cs
    ├── VehicleTests.cs
    └── VehiclesManagementFormBusinessTests.cs
```

## Branching Strategy

BusBuddy follows a tailored GitFlow branching model:

### Long-lived Branches
- **main**: Production-ready code, protected branch
- **develop**: Integration branch for features

### Short-lived Branches
- **feature/form-{form-name}**: For UI form development (e.g., `feature/form-vehicle-management`)
- **feature/model-{entity}**: For model/entity changes (e.g., `feature/model-driver-licensing`)
- **feature/data-{component}**: For database-related changes (e.g., `feature/data-migration-fix`)
- **bugfix/{issue-number}-{short-description}**: For bug fixes (e.g., `bugfix/423-fuel-calculation`)
- **release/v{major}.{minor}.{patch}**: For release preparation (e.g., `release/v2.1.0`)
- **hotfix/v{major}.{minor}.{patch+1}**: For emergency fixes (e.g., `hotfix/v2.1.1`)

### Development Workflow

1. **Start New Development**
   ```pwsh
   git checkout develop
   git pull
   git checkout -b feature/form-vehicle-management
   # Make changes...
   git add .
   git commit -m "Add fuel tracking to vehicle management form"
   git push -u origin feature/form-vehicle-management
   ```

2. **Create Pull Request** (via GitHub)
   - Navigate to Pull Requests > New Pull Request
   - Select base:develop and compare:feature/form-vehicle-management
   - Add description and request review

3. **Release Process**
   ```pwsh
   # After features are merged to develop
   git checkout develop
   git pull
   git checkout -b release/v1.1.0
   # Final testing and version updates...
   git add .
   git commit -m "Prepare release v1.1.0"
   git push -u origin release/v1.1.0
   ```

4. **Finalize Release** (via Release Management workflow)
   - Run the Release Management workflow with "finalize-release" option

For more details, see the [branch-strategy.yml](/.github/workflows/branch-strategy.yml) workflow.

## Automated Workflows

Our project uses several GitHub Actions workflows for automation:

- **CI**: Automatic build and test for protected branches
  - Triggers on: push to main/develop/release*/hotfix*, PRs to main/develop
  - Runs: restore, build, test

- **Feature Branch Workflow**: Quality checks for feature development
  - Triggers on: push to feature*/bugfix*, PRs to develop
  - Runs: build, test, code quality analysis

- **Auto Sync**: Keeps branches in sync
  - Triggers: Daily at midnight, manual trigger
  - Options: Sync main or develop branch

- **Release Management**: Handles GitFlow release processes
  - Options: Create/finalize release branch, create/finalize hotfix
  - Requires: Version number input

- **Auto Version and Release**: Smart versioning based on commits
  - Version bump keywords: "breaking"/"major", "feature"/"minor"
  - Skip release with: "[no-release]" in commit message

## Technical Overview

### Technology Stack
- **Frontend**: Windows Forms with MaterialSkin.2 UI library
- **Backend**: .NET 8.0 with Entity Framework Core
- **Database**: SQL Server Express
- **Logging**: Microsoft.Extensions.Logging with file output
- **Testing**: xUnit with Moq for unit tests
- **CI/CD**: GitHub Actions workflows

### Entity Relationship Handling
The system implements proper relationship handling to maintain data integrity:

- **Foreign Key Constraints**: All entity relationships are configured with appropriate DeleteBehavior
- **Safe Driver Deletion**: Custom implementation to handle FK_RouteData_Drivers_PMDriverId constraint
- **Reassignment Option**: When deleting entities, users can reassign dependent records instead of nullifying references
- **Transactional Operations**: All data modifications use transactions to ensure consistency

### Key Metrics
- **Classes**: 47
- **Forms**: 16
- **Untested Components**: 31 (e.g., `RouteManagementForm.cs`, `VehiclesManagementForm.cs`)
- **Architecture Violations**: 22 (e.g., missing `[Required]` in `Models\Entities\Driver.cs`)

## Database Schema Notes & Fixes

### Foreign Key Constraint Fix (May 10, 2025)
The application previously encountered a foreign key constraint error when creating the database:

```
Introducing FOREIGN KEY constraint 'FK_RouteData_Drivers_PMDriverId' on table 'RouteData' 
may cause cycles or multiple cascade paths.
```

This error occurred because the `RouteData` entity had two foreign key relationships to the `Driver` entity 
(AMDriverId and PMDriverId), both using `DeleteBehavior.SetNull`, creating multiple cascade paths.

#### Solution
1. Changed the `PMDriverId` relationship from `DeleteBehavior.SetNull` to `DeleteBehavior.Restrict`:

```csharp
modelBuilder.Entity<RouteData>()
    .HasOne(rd => rd.PMDriver)
    .WithMany()
    .HasForeignKey(rd => rd.PMDriverId)
    .OnDelete(DeleteBehavior.Restrict); // Changed from SetNull to Restrict
```

2. Enhanced the `DeleteDriverSafelyAsync` method to explicitly handle the `RouteData` references.

#### Testing
- Created unit tests to verify both driver reassignment and nullification work correctly
- Verified database creation succeeds with the updated configuration
- Confirmed that driver deletion with related `RouteData` records works properly

### Core Features
- **Driver Management**: Track driver information, licenses, certifications
  - Safe driver deletion with options to reassign or remove dependent records
  - License expiration tracking and alerts
- **Vehicle Management**: Monitor bus fleet, maintenance, fuel consumption
- **Route Planning**: Optimize school bus routes and schedules
- **Activity Trips**: Manage special events and field trips
- **Maintenance Tracking**: Schedule and document vehicle maintenance
- **Compliance Reporting**: Generate reports for regulatory compliance
- **Safe Data Operations**: Transactional updates with proper constraint handling

## Development Guidelines

### Branch Naming Conventions
- **Feature branches**: `feature/area-description` (e.g., `feature/form-route-editor`)
- **Bug fixes**: `bugfix/number-description` (e.g., `bugfix/423-null-reference`)
- **Release branches**: `release/vX.Y.Z` (e.g., `release/v1.2.0`)
- **Hotfix branches**: `hotfix/vX.Y.Z` (e.g., `hotfix/v1.2.1`)

### Coding Standards
- **Modern UI**: MaterialSkin.2 for dark-themed forms with consistent navigation
- **Model Validation**: Required attributes and nullable annotations on entity models
- **Error Handling**: Structured exception handling with logging
- **Testing**: Unit tests required for all new business logic
- **Database Access**: Use EF Core for data access, avoid direct SQL except in legacy code

## Setup & Development

### Prerequisites
- **Required**: 
  - Windows 10/11
  - .NET 8.0 SDK or newer
  - SQL Server Express 2022
  - Visual Studio 2022+ or VS Code

- **Recommended**:
  - Git for Windows
  - SQL Server Management Studio

### Getting Started

1. **Clone the repository:**
   ```pwsh
   git clone https://github.com/Bigessfour/BusBuddy.git
   cd BusBuddy/BusBuddy
   ```

2. **Set up branch structure:**
   ```pwsh
   # If develop branch doesn't exist:
   git checkout -b develop
   git push -u origin develop
   ```

3. **Restore NuGet packages:**
   ```pwsh
   dotnet restore
   ```

4. **Configure database connection:**
   - Edit `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "BusBuddyDatabase": "Server=localhost\\SQLEXPRESS;Database=BusBuddy;Trusted_Connection=True;TrustServerCertificate=True"
     }
   }
   ```

5. **Initialize database:**
   ```pwsh
   dotnet ef database update
   ```

6. **Build and run:**
   ```pwsh
   dotnet build
   dotnet run
   ```

### Common Development Tasks

1. **Create a new feature branch:**
   ```pwsh
   git checkout develop
   git pull
   git checkout -b feature/form-new-feature
   ```

2. **Update database after model changes:**
   ```pwsh
   dotnet ef migrations add DescriptiveNameOfChange
   dotnet ef database update
   ```

3. **Run tests:**
   ```pwsh
   dotnet test Tests/BusBuddy.Tests.csproj
   ```

4. **Check test coverage:**
   ```pwsh
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
   ```

## Development Roadmap

### Current Sprint (May 2025)
- Complete driver management forms
- Increase test coverage to 25%
- Implement branch protection rules
- Resolve remaining architecture violations

### Future Plans
- Implement real-time GPS tracking
- Add mobile companion app integration
- Develop analytics dashboard
- Create comprehensive reporting system

## Testing
- Run all xUnit tests:
  ```pwsh
  dotnet test
  ```

## Contribution Guidelines
- Follow the Core Build Guidelines.
- Test all changes with xUnit before submitting a PR.
- Log all feature additions and errors using `Microsoft.Extensions.Logging` to `busbuddy_errors.log`.
- Document all changes and update `architecture_violations.json` as needed.

## Logging
- All errors and feature additions are logged to `busbuddy_errors.log` via `ILogger`.

## Troubleshooting and Maintenance
- For common issues and solutions, see [TROUBLESHOOTING.md](./TROUBLESHOOTING.md)
- Regular database backups should be scheduled using [BackupDatabase.ps1](./BackupDatabase.ps1)
- For database schema details and relationship handling, see [DATABASE.md](./DATABASE.md)

## Docker Support
BusBuddy supports using a containerized SQL Server database for development and testing.

### Prerequisites
- Docker Desktop installed and running
- PowerShell 7.0 or later

### Using Docker Database
1. **Start the Docker database**:
   ```pwsh
   .\Switch-DatabaseMode.ps1 -Mode docker
   ```
   This will:
   - Set the environment variable to use Docker
   - Start the SQL Server container if not running
   - Configure the application to use the DockerConnection string

2. **Switch back to local database**:
   ```pwsh
   .\Switch-DatabaseMode.ps1 -Mode local
   ```

3. **Advanced Docker Commands**:
   ```pwsh
   # Start all Docker services
   docker-compose up -d
   
   # Start with development overrides
   docker-compose -f docker-compose.yml -f docker-compose.dev.yml up -d
   
   # View container logs
   docker-compose logs -f
   
   # Stop all containers
   docker-compose down
   ```

4. **Connection Information**:
   - Docker database server: `localhost,1433`
   - Database: `BusBuddy`
   - User: `BusBuddyApp`
   - Password: `App@P@ss!2025`

---
For more details, see the in-code comments and documentation.