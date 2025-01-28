# Use the official image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Set the working directory inside the container
WORKDIR /app

# Expose the port your application will run on (default is 80 for web apps)
EXPOSE 80

# Use the SDK image to build the app (use .NET 8 SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the project file and restore the dependencies
COPY ["email-sender.csproj", "./"]

# Restore the NuGet packages
RUN dotnet restore "email-sender.csproj"

# Copy the rest of the files and build the application
COPY . .
WORKDIR "/src"
RUN dotnet build "email-sender.csproj" -c Release -o /app/build

# Publish the app to the /app/publish directory
RUN dotnet publish "email-sender.csproj" -c Release -o /app/publish

# Set the working directory for the runtime
FROM base AS final
WORKDIR /app

# Copy the published files from the build image
COPY --from=build /app/publish .

# Set the entry point to run your application
ENTRYPOINT ["dotnet", "email-sender.dll"]
