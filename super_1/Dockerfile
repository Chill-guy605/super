# Base image for building the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["super/super_1.csproj", "super_1/"]
RUN dotnet restore "super/super_1.csproj"

# Copy the rest of the project files
COPY . .

# Set working directory to the project folder and publish the app
WORKDIR "/src/super_1"
RUN dotnet publish "super_1.csproj" -c Release -o /app/publish

# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the published files from the build image
COPY --from=build /app/publish .

# Set entry point for the application
ENTRYPOINT ["dotnet", "super_1.dll"]
