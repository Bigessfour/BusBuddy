# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project files first and restore dependencies
# This creates a separate layer that won't change unless the project files change
COPY ["BusBuddy.csproj", "./"]
RUN dotnet restore "BusBuddy.csproj"

COPY ["Tests/BusBuddy.Tests.csproj", "Tests/"]
RUN dotnet restore "Tests/BusBuddy.Tests.csproj"

# Copy only necessary source code for building (exclude unnecessary files)
COPY ["*.cs", "./"]
COPY ["Controllers/", "Controllers/"]
COPY ["Models/", "Models/"]
COPY ["Services/", "Services/"]
COPY ["Views/", "Views/"]
COPY ["wwwroot/", "wwwroot/"]
COPY ["Properties/", "Properties/"]
COPY ["Tests/*.cs", "Tests/"]

# Build and publish the application
RUN dotnet build "BusBuddy.csproj" -c Release -o /app/build
RUN dotnet publish "BusBuddy.csproj" -c Release -o /app/publish

# Add health check for container monitoring
HEALTHCHECK --interval=30s --timeout=3s \
  CMD curl -f http://localhost:80/ || exit 1

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Copy only the published output
COPY --from=build /app/publish .

# Create non-root user for better security
USER 1000

# Set environment to Production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port for the application
EXPOSE 80

ENTRYPOINT ["dotnet", "BusBuddy.dll"]
