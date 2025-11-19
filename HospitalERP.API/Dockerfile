# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install curl for healthchecks first for better layer caching
# This layer will be cached unless the base image changes
RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/* && \
    apt-get clean

WORKDIR /src

# Copy only project file first for better layer caching
# This layer will be cached unless dependencies change
COPY HospitalERP.API.csproj .
RUN dotnet restore --use-current-runtime

# Copy all source code (this layer will invalidate cache when code changes)
COPY . .

# Publish stage - builds and publishes in one step for better caching
FROM build AS publish
RUN dotnet publish -c Release --no-restore -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app


EXPOSE 5037
EXPOSE 443

# Copy published app from publish stage
# This layer invalidates when application code changes
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "HospitalERP.API.dll"]

