# Base image til runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK image til build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["FitnessAppAPI/FitnessAppAPI.csproj", "FitnessAppAPI/"]
RUN dotnet restore "FitnessAppAPI/FitnessAppAPI.csproj"
COPY . .
WORKDIR "/src/FitnessAppAPI"
RUN dotnet build "FitnessAppAPI.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "FitnessAppAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Tilføj et lille script, der kører migrationer, før applikationen starter
RUN echo '#!/bin/bash\n\
set -e\n\
echo "Venter på database..."\n\
sleep 10\n\
echo "Kører migrations..."\n\
dotnet FitnessAppAPI.dll --migrate\n\
echo "Starter applikation..."\n\
exec dotnet FitnessAppAPI.dll' > /app/entrypoint.sh \
    && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
