# File Organization Strategy

## Folder Structure

The BusBuddy application uses the following folder organization:

- `/Scripts/` - Contains all PowerShell (.ps1) scripts used for automation, deployment, and configuration
- `/Docs/` - Contains all documentation in Markdown (.md) format

## Why This Structure?

1. **Separation of Concerns**: Keeping scripts and documentation separate from application code improves readability and maintainability
2. **Ease of Discovery**: Developers can quickly find relevant documentation or automation scripts
3. **Consistent Standards**: Following industry best practices for project organization

## Maintaining Organization

To maintain proper organization:

1. Place any new PowerShell scripts in the `/Scripts/` folder
2. Place any new documentation in the `/Docs/` folder
3. Run the `OrganizeProjectFiles.ps1` script periodically to ensure files are in their proper locations

## Adding New Types of Files

If there's a need to organize additional file types:
1. Update the `OrganizeProjectFiles.ps1` script
2. Document the new organization policy in this document
3. Update the project file (.csproj) to include the new folder structure

## Execution Instructions

To execute the file organization process:

1. Run `Scripts\ExecuteOrganization.bat` from the project root
2. The script will automatically:
   - Create necessary directories if they don't exist
   - Move all .ps1 files to the Scripts directory
   - Move all .md files to the Docs directory
3. Review the console output to ensure all files were moved correctly

This process should be run periodically, especially after adding new documentation or scripts.
