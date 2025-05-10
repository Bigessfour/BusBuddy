# BusBuddy Database Schema

This document outlines the database schema for the BusBuddy application, including entity relationships, constraints, and specific design considerations.

## Entity Relationships

### Driver Entity

The `Driver` entity is related to several other entities:

1. `RouteData.AMDriverId` → `Driver.Id` (SetNull)
2. `RouteData.PMDriverId` → `Driver.Id` (Restrict)
3. `Vehicle.AssignedDriverId` → `Driver.Id` (SetNull)
4. `FuelEntry.DriverId` → `Driver.Id` (None/Null)
5. `ActivityTrip.DriverId` → `Driver.Id` (Cascade)

### Special Considerations

#### Multiple Foreign Key Relationships

When an entity has multiple foreign key relationships to the same entity (e.g., `RouteData` has both `AMDriverId` and `PMDriverId` referencing `Driver`), SQL Server can encounter a "multiple cascade paths" error. This occurs when:

1. Multiple relationships exist between the same tables
2. Both relationships use `CASCADE` behavior (either `DELETE` or `UPDATE`)

#### Fixed Relationships

To avoid multiple cascade paths, we've configured the `RouteData` relationships as follows:

```csharp
// First relationship uses SetNull
modelBuilder.Entity<RouteData>()
    .HasOne(rd => rd.AMDriver)
    .WithMany()
    .HasForeignKey(rd => rd.AMDriverId)
    .OnDelete(DeleteBehavior.SetNull);

// Second relationship uses Restrict to avoid multiple cascade paths
modelBuilder.Entity<RouteData>()
    .HasOne(rd => rd.PMDriver)
    .WithMany()
    .HasForeignKey(rd => rd.PMDriverId)
    .OnDelete(DeleteBehavior.Restrict);
```

## Safe Driver Deletion

Driver deletion is handled through a specific method that manages all related entities:

```csharp
// BusBuddyContext.cs
public async Task<bool> DeleteDriverSafelyAsync(int driverId, int? reassignToDriverId = null, ILogger? logger = null)
```

This method provides two options for handling related data:

1. **Reassign**: Provide another driver ID to reassign all related records to that driver
2. **Nullify/Remove**: Leave reassignToDriverId as null to set references to null (or delete, depending on configuration)

### Handling Process

For each related entity, the method:

1. Fetches all related records
2. Updates the references based on whether reassignment is requested
3. Performs the operation within a transaction to ensure consistency

#### Example Usage

```csharp
// Reassign all of driver 1's records to driver 2
await context.DeleteDriverSafelyAsync(1, 2, logger);

// Delete driver 3 and set references to null
await context.DeleteDriverSafelyAsync(3, null, logger);
```

## Entity Framework Core DeleteBehavior Options

When configuring relationships, these are the available behaviors:

- **Cascade**: Automatically delete dependent entities when the principal is deleted
- **ClientSetNull**: Set FKs to null in memory, but don't cascade at database level
- **Restrict/NoAction**: Prevent deletion if related data exists
- **SetNull**: Automatically set dependent FKs to null when the principal is deleted

## Migrations

Migrations are used to version and update the database schema. When adding new entities or modifying relationships:

1. Ensure proper DeleteBehavior is configured in `OnModelCreating`
2. Create a new migration: `dotnet ef migrations add {MigrationName}`
3. Review the migration for potential issues
4. Apply the migration: `dotnet ef database update`

## Common Issues and Solutions

### Multiple Cascade Paths

**Error**: `Introducing FOREIGN KEY constraint 'FK_X_Y' may cause cycles or multiple cascade paths`

**Solution**:
1. Identify the relationships causing the issue
2. Change one of the relationships to use `Restrict/NoAction` instead of `Cascade/SetNull`
3. Handle the modified relationship manually in code

### Constraint Violations

**Error**: `The DELETE statement conflicted with the REFERENCE constraint 'FK_X_Y'`

**Solution**:
1. Use the safe deletion methods provided in the application
2. Ensure all related data is handled before deletion
