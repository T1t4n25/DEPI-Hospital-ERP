#!/bin/bash
# Start Docker containers for Hospital ERP
# Usage: ./scripts/docker-start.sh

set -e

echo "Starting Docker containers..."

# Check if .env file exists
if [ ! -f .env ]; then
    echo "Warning: .env file not found. Creating from .env.example..."
    if [ -f .env.example ]; then
        cp .env.example .env
        echo "Created .env file. Please edit it with your settings."
    else
        echo "Error: .env.example not found!"
        exit 1
    fi
fi

# Load environment variables
source .env 2>/dev/null || true

# Start containers
docker-compose up -d

echo "Waiting for SQL Server to be ready..."
sleep 5

# Wait for SQL Server to be ready
timeout=60
elapsed=0
while ! docker-compose exec -T sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "${DB_PASSWORD}" -Q "SELECT 1" &>/dev/null; do
    if [ $elapsed -ge $timeout ]; then
        echo "Timeout waiting for SQL Server to be ready"
        docker-compose logs sqlserver
        exit 1
    fi
    echo "Waiting for SQL Server... ($elapsed/$timeout seconds)"
    sleep 2
    elapsed=$((elapsed + 2))
done

echo "SQL Server is ready!"
echo "Database: ${DB_DATABASE:-HospitalERP}"
echo "Server: ${DB_SERVER:-localhost}:${DB_PORT:-1433}"
echo ""
echo "Run migrations with: cd HospitalERP.API && dotnet ef database update"

