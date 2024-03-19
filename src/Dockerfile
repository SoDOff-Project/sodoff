# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the source code
COPY . .

# Restore dependencies and build the application
RUN dotnet build -c Release -o /app

# move mods and assets directors from sources to /app
RUN mv src/mods /app && ln -s /app/mods src/
RUN mv src/assets /app && ln -s /app/assets src/

# Create clean run environment (without source and sdk)
# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
# WORKDIR /app
# COPY --from=build /app .

# Run the application
WORKDIR /app
ENTRYPOINT ["./sodoff"]
