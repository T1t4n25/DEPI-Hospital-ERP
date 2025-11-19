# Hospital ERP - Docker Compose Guide

This project uses Docker Compose for containerization. There are three docker-compose files:

1. **Root `docker-compose.yml`** - Orchestrates the entire application (backend + frontend)
2. **`HospitalERP.API/docker-compose.yml`** - Backend services only (API, SQL Server, Keycloak)
3. **`frontend/docker-compose.yml`** - Frontend service only

## Quick Start

### Option 1: Run Everything (Recommended)

Run all services from the project root:

```bash
# Start all services
docker compose up -d

# View logs
docker compose logs -f

# Stop all services
docker compose down

# Stop and remove volumes (WARNING: deletes data)
docker compose down -v
```

### Option 2: Run Backend Only

From the `HospitalERP.API` directory:

```bash
cd HospitalERP.API
docker compose up -d
```

This starts:
- SQL Server
- Keycloak
- .NET API

### Option 3: Run Frontend Only

From the `frontend` directory:

```bash
cd frontend
docker compose up -d
```

This starts:
- Angular Frontend (nginx)

**Note:** The frontend will need the backend API running separately for full functionality.

## Services

### Backend Services

- **SQL Server**: `localhost:1433` (default port)
- **Keycloak**: `localhost:8080`
- **.NET API**: `localhost:5037`

### Frontend Service

- **Angular Frontend**: `localhost:4200` (default port)

## Environment Variables

### Backend Environment File

Create `HospitalERP.API/.env` file:

```env
# Database
DB_PASSWORD=YourStrong!Password123
DB_PORT=1433
DB_SERVER=localhost
DB_DATABASE=HospitalERP
DB_USER=sa

# Keycloak
KEYCLOAK_PORT=8080
KEYCLOAK_ADMIN=admin
KEYCLOAK_ADMIN_PASSWORD=admin
KEYCLOAK_AUTHORITY=http://localhost:8080/realms/hospital-erp
KEYCLOAK_AUDIENCE=hospital-erp-api,account

# API
API_PORT=5037
```

### Frontend Environment File

Create `frontend/.env` file (optional):

```env
# Frontend
FRONTEND_PORT=4200

# API Connection (for browser - use localhost with mapped ports)
API_URL=http://localhost:5037
KEYCLOAK_URL=http://localhost:8080
KEYCLOAK_REALM=hospital-erp
KEYCLOAK_CLIENT_ID=hospital-frontend
```

### Root Environment Variables

You can override any of the above by setting environment variables in your shell or using a `.env` file in the root directory.

## Network Configuration

All services run on the `hospitalerp-network` Docker network, allowing them to communicate using service names:

- Frontend → API: `http://api:80`
- API → SQL Server: `sqlserver:1433`
- API → Keycloak: `http://keycloak:8080`

**Note:** For browser-based API calls from the frontend, use `localhost` with mapped ports (e.g., `http://localhost:5037`) since browsers cannot access Docker internal networks.

## Volumes

The following volumes are created for data persistence:

- `sqlserver_data` - SQL Server database files
- `keycloak_data` - Keycloak data
- `keycloak_providers` - Keycloak providers
- `keycloak_themes` - Keycloak themes

## Health Checks

All services have health checks configured:

- **SQL Server**: Checks database connectivity
- **Keycloak**: Checks `/health/ready` endpoint
- **API**: Checks `/swagger` endpoint
- **Frontend**: Checks `/health` endpoint

Services wait for dependencies to be healthy before starting.

## Building Images

### Build All Services

```bash
# From project root
docker compose build
```

### Build Specific Service

```bash
# Backend API
docker compose build api

# Frontend
docker compose build frontend
```

## Development Workflow

### Hot Reload (Development)

For development with hot reload:

**Backend:**
```bash
cd HospitalERP.API
dotnet watch run
```

**Frontend:**
```bash
cd frontend
npm start
```

Then use Docker Compose only for infrastructure services (SQL Server, Keycloak):

```bash
# Start only infrastructure
docker compose up -d sqlserver keycloak
```

### Production Build

Build and run production images:

```bash
# Build production images
docker compose build

# Run in production mode
docker compose up -d
```

## Troubleshooting

### View Service Logs

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f api
docker compose logs -f frontend
```

### Check Service Status

```bash
docker compose ps
```

### Restart a Service

```bash
docker compose restart api
docker compose restart frontend
```

### Access Container Shell

```bash
# API container
docker compose exec api sh

# Frontend container
docker compose exec frontend sh
```

### Clear All Data

**WARNING:** This will delete all database data and volumes!

```bash
docker compose down -v
docker volume prune -f
```

## Port Conflicts

If ports are already in use, you can change them in the `.env` files:

```env
DB_PORT=1434          # Change SQL Server port
KEYCLOAK_PORT=8081    # Change Keycloak port
API_PORT=5038         # Change API port
FRONTEND_PORT=4201    # Change Frontend port
```

## First-Time Setup

1. **Create environment files:**
   ```bash
   # Copy example files if available
   cp HospitalERP.API/.env.example HospitalERP.API/.env
   cp frontend/.env.example frontend/.env
   ```

2. **Start infrastructure services:**
   ```bash
   docker compose up -d sqlserver keycloak
   ```

3. **Wait for services to be healthy:**
   ```bash
   docker compose ps
   ```

4. **Run database migrations:**
   ```bash
   cd HospitalERP.API
   dotnet ef database update
   ```

5. **Configure Keycloak:**
   - Access Keycloak at `http://localhost:8080`
   - Login with admin credentials
   - Create realm: `hospital-erp`
   - Create client: `hospital-erp-api`
   - Create user for testing

6. **Start all services:**
   ```bash
   docker compose up -d
   ```

## Accessing Services

- **Frontend**: http://localhost:4200
- **API**: http://localhost:5037
- **API Swagger**: http://localhost:5037/swagger
- **Keycloak**: http://localhost:8080
- **Keycloak Admin**: http://localhost:8080/admin
- **SQL Server**: localhost:1433

## Notes

- The frontend Docker image uses nginx to serve static files
- The Angular app is built at container build time
- Environment variables for Angular need to be set at build time (or use runtime substitution with nginx)
- For development, consider running Angular with `ng serve` for hot reload instead of Docker

