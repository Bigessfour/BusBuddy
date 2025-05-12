# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["BusBuddy.csproj", "./"]
COPY ["Directory.Packages.props", "./"]
RUN dotnet restore "BusBuddy.csproj"

# Copy necessary files (excluding Forms, BusBuddyApplicationContext.cs, and Windows Forms dependencies)
COPY ["Pages/", "Pages/"]
COPY ["Controllers/", "Controllers/"]
COPY ["Data/", "Data/"]
COPY ["Services/", "Services/"]
COPY ["DTOs/", "DTOs/"]
COPY ["Hubs/", "Hubs/"]
COPY ["wwwroot/", "wwwroot/"]
COPY ["Program.cs", "./"]
COPY ["appsettings.json", "./"]
COPY ["appsettings.Development.json", "./"]

# Build and publish the application
RUN dotnet build "BusBuddy.csproj" -c Release -o /app/build
RUN dotnet publish "BusBuddy.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=build /app/publish .

# Create directory for SQLite database
RUN mkdir -p /data && chmod 777 /data

# Add healthcheck
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost/health || exit 1

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV USE_SQLITE=true
ENV ASPNETCORE_URLS=http://+:80

# Expose port
EXPOSE 80

# Volume for persistent data
VOLUME ["/data"]

# Start the app
ENTRYPOINT ["dotnet", "BusBuddy.dll"]

ENTRYPOINT ["dotnet", "BusBuddy.dll"]
