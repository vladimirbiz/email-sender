# Use the .NET 8 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["email-sender.csproj", "./"]
RUN dotnet restore "email-sender.csproj"

# Copy the rest of the source code (this includes MainController.cs and other C# files)
COPY . .

# Build the app
RUN dotnet build "email-sender.csproj" -c Release -o /app/build

# Publish the app to the /app/publish directory
FROM build AS publish
RUN dotnet publish "email-sender.csproj" -c Release -o /app/publish

# Use the .NET 8 runtime image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Copy the app from the publish stage (compiled binaries)
COPY --from=publish /app/publish .

# Copy the test.csv file to the container (ensure it's in the /app directory)
COPY test.csv /app/test.csv  

COPY MainController.cs /app/MainController.cs 

# Set the entrypoint to run the app
ENTRYPOINT ["dotnet", "email-sender.dll"]

