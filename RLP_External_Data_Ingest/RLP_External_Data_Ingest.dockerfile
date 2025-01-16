# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Copy the project file and restore dependencies
COPY *.csproj .
RUN dotnet restore

# Copy the rest of the application code and build the application
COPY . .
RUN dotnet publish -c Release -o /app

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "RLP_External_Data_Ingest.dll"]