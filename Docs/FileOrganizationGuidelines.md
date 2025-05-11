# File Organization Guidelines

## Current Structure

The BusBuddy project follows an organized file structure with dedicated locations for different file types:

- **PowerShell Scripts (.ps1)**: All located in the `/Scripts` folder
- **Documentation (.md)**: All located in the `/Docs` folder

## Maintaining This Structure

### For Developers

When adding new files to the project:

1. Place all PowerShell scripts directly in the `/Scripts` folder
2. Place all documentation files directly in the `/Docs` folder
3. Run the organization script periodically to ensure compliance:

```powershell
& "c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy\Scripts\OrganizeFiles.ps1"
```

### Organization Benefits

- **Improved Discoverability**: Finding scripts and documentation is easier when they're in dedicated locations
- **Cleaner Project Root**: Keeping specialized files in their own directories reduces clutter
- **Better Collaboration**: Team members know exactly where to find and place these file types
- **Easier Maintenance**: Updates and management of scripts and documentation are simplified

## Automation

The project includes tools to help maintain this organization:

1. **GetFileStructure.ps1**: Generates a report of the current project structure
2. **OrganizeFiles.ps1**: Automatically moves .ps1 and .md files to their proper locations
3. **RunFileStructure.bat**: Batch file to easily run the file structure report generator

## Visual Studio Integration

For Visual Studio users, the project includes proper folder organization in the solution explorer. The folders are included in the .csproj file to ensure they're properly recognized by the IDE.

## Best Practices

1. **Documentation File Naming**: Use descriptive names for documentation files that clearly indicate their content
2. **Script Naming**: Name scripts according to their function using verb-noun format (e.g., Get-ProjectInfo.ps1)
3. **Keep Organized**: Run the organization script periodically, especially after adding new files
4. **Update References**: If you move a file that's referenced elsewhere, make sure to update those references

This organization system helps maintain a clean, professional project structure that improves development efficiency and project management.
