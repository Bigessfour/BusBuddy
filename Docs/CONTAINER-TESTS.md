# BusBuddy Containerized Tests

This document explains how to run BusBuddy tests in Docker containers.

## Requirements

- Docker Desktop
- Docker Compose
- .NET 6 SDK (for local development)

## Running Tests in Docker

To run the tests in Docker:

1. Open a command prompt in the BusBuddy project directory
2. Run the batch script: `.\RunTestsInDocker.bat`
3. Test results will be saved in the `TestResults` directory

## Test Categories

The BusBuddy test suite contains both containerizable and Windows-only tests:

- **Containerizable Tests**: These are mostly business logic, service, and entity tests that don't have Windows dependencies.
- **Windows-Only Tests**: These are UI tests that require Windows Forms and can only run on Windows. These are marked with the `[WindowsOnly]` attribute.

When running in Docker, only the containerizable tests will be executed.

## Customizing the Test Run

You can customize the test run by modifying the `docker-compose.tests.yml` file:

- Change SQL Server settings
- Add more test environment variables
- Configure test filtering options

## Troubleshooting

If you encounter issues:

1. Make sure Docker is running
2. Check that ports 1434 is not in use by another application
3. Examine the Docker logs for detailed error messages
4. Try running the tests locally with `dotnet test` to see if the issue is specific to containerization
