# Use the official .NET 8 runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/ApiGetwate/ApiGetwate.csproj", "src/ApiGetwate/"]
COPY ["src/SharedLibrary/SharedLibrary.csproj", "src/SharedLibrary/"]

# Restore dependencies
RUN dotnet restore "src/ApiGetwate/ApiGetwate.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/ApiGetwate"
RUN dotnet build "ApiGetwate.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ApiGetwate.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "ApiGetwate.dll"]
