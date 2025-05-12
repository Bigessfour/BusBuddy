# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj, solution, and central package management
COPY ["BusBuddy.csproj", "./"]
COPY ["Directory.Packages.props", "./"]
COPY ["BusBuddy.sln", "./"]
COPY ["global.json", "./"]

# Restore dependencies for the main app
RUN dotnet restore "BusBuddy.csproj"

# Copy only the necessary components for a web application
COPY ["Pages/", "Pages/"]
COPY ["Components/", "Components/"]
COPY ["Controllers/", "Controllers/"]
COPY ["DTOs/", "DTOs/"]
COPY ["Services/", "Services/"]
COPY ["Models/", "Models/"]
COPY ["Data/", "Data/"]
COPY ["Entities/", "Entities/"]
COPY ["Hubs/", "Hubs/"]
COPY ["Infrastructure/", "Infrastructure/"]
COPY ["wwwroot/", "wwwroot/"]
COPY ["App.razor", "./"]
COPY ["_Imports.razor", "./"]
COPY ["WebStartup.cs", "./"]
COPY ["Program.cs.blazor", "Program.cs"]
COPY ["appsettings.json", "./"]
COPY ["appsettings.Development.json", "./"]

# Build and publish the application (web components only)
RUN dotnet build "BusBuddy.csproj" -c Release -o /app/build /p:UseWindowsForms=false
RUN dotnet publish "BusBuddy.csproj" -c Release -o /app/publish /p:UseAppHost=false /p:UseWindowsForms=false
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Add healthcheck for container monitoring
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost:5000/health || exit 1

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5000
ENV USE_SQLITE=true

# Create a volume for SQLite data if needed
VOLUME /app/data

EXPOSE 5000

ENTRYPOINT ["dotnet", "BusBuddy.dll"]
