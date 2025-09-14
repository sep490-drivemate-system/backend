FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/UserService/UserService.csproj", "src/UserService/"]
COPY ["src/SharedLibrary/SharedLibrary.csproj", "src/SharedLibrary/"]
RUN dotnet restore "src/UserService/UserService.csproj"

# Copy source code and build
COPY src/ src/
WORKDIR /src/src/UserService
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "UserService.dll"]