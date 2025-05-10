# BusBuddyMVP

A .NET 9.0 Windows Forms application for managing school bus operations, built with SQL Server Express, Entity Framework Core, MaterialSkin.2 for UI, and Microsoft.Extensions.Logging for diagnostics.

## Current Status (as of May 8, 2025)
- **Build Status**: Failing
- **Errors**: 5 (CS0246 in `Data\Interfaces\IDatabaseHelper.cs` due to missing `System` namespace)
- **Warnings**: 3 (nullability mismatches in `Logging\FileLogger.cs` and `Controls\MaterialDataGridView.cs`)
- **Current Floor**: 38 (stable base, addressing foundational errors)
- **Progress**: Fixing build errors and planning UI/functional enhancements

## Project Health
- **Build Status**: 5 errors, 3 warnings
- **Test Coverage**: 12.12% (31 untested components)
- **Architectural Health**: Poor (22 violations, primarily model nullability and missing entity IDs)
- **Log Errors**: 8 warnings logged, no recent errors

## Key Metrics
- **Classes**: 47
- **Forms**: 16
- **Untested Components**: 31 (e.g., `RouteManagementForm.cs`, `VehiclesManagementForm.cs`)
- **Architecture Violations**: 22 (e.g., missing `[Required]` in `Models\Entities\Driver.cs`)

## Core Build Guidelines
To ensure stability and scalability, adhere to these principles:
- **Modern UI**: Use MaterialSkin.2 for dark-themed, responsive forms (e.g., `Dashboard.cs`). Implement navigation sidebars and tab controls for improved UX.
- **Real-Time Tracking**: Add placeholders for GPS integration in `Dashboard.cs` and `RouteManagementForm.cs` (e.g., `mapPanel`) until libraries like GMap.NET are validated.
- **Automated Scheduling**: Plan schedule logic in `RouteManagementForm.cs` with placeholders for notifications, avoiding database changes until tested.
- **Mobile Accessibility**: Design forms with future web/mobile compatibility, avoiding desktop-specific layouts.
- **Safety/Compliance**: Implement validation in `DriversManagementForm.cs` and `VehiclesManagementForm.cs` for license/insurance expirations using `DatabaseHelper.cs`.
- **Data-Driven Insights**: Plan analytics in `Dashboard.cs` or a future `ReportsForm.cs` with placeholders for charts (e.g., fuel trends) until Chart controls are added.
- **Stability**: Preserve existing database logic (`BusBuddyContext.cs`) and dependencies. Test changes incrementally to maintain 0 errors where possible.
- **Continuity**: Log all UI/feature additions to `busbuddy_errors.log` via `ILogger` for tracking.

## Recommendations
- **Fix Build Errors**: Add `using System;` to `Data\Interfaces\IDatabaseHelper.cs` to resolve 5 CS0246 errors.
- **Increase Test Coverage**: Add unit tests for 31 untested components, prioritizing `RouteManagementForm.cs` and `VehiclesManagementForm.cs`.
- **Address Violations**: Add `[Required]` attributes or nullable annotations to string properties in `Models\Entities\*.cs` and ensure entity classes have `Id` properties.
- **Enhance UI**: Implement a `MaterialTabControl` in `Dashboard.cs` for tracking and analytics placeholders.

## Getting Started
1. Clone the repository.
2. Restore NuGet packages (e.g., MaterialSkin.2, Microsoft.EntityFrameworkCore.SqlServer).
3. Configure SQL Server Express connection in `appsettings.json`.
4. Build and run via Visual Studio 2022+ with .NET 9.0 SDK.
5. Apply the `using System;` fix to `IDatabaseHelper.cs` to resolve build errors.

## Contributing
- Follow the Core Build Guidelines.
- Test changes using xUnit (see `Tests\DatabaseHelperTests.cs`).
- Log issues to `busbuddy_errors.log` and update `architecture_violations.json` for tracking.