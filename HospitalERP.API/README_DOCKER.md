# Docker Setup for Hospital ERP

This guide explains how to set up and run the Hospital ERP database using Docker.

## Prerequisites

- Docker Desktop installed and running
- Docker Compose installed
- .NET 9.0 SDK (for running migrations)

## Quick Start

1. **Create `.env` file** (copy from `.env.example` if not exists):
   ```bash
   cp .env.example .env
   ```

2. **Edit `.env` file** with your preferred database password:
   ```env
   DB_PASSWORD=YourStrong!Password123
   ```

3. **Start the SQL Server container**:
   ```bash
   docker-compose up -d
   ```

4. **Wait for SQL Server to be ready** (check logs):
   ```bash
   docker-compose logs -f sqlserver
   ```
   Wait until you see: `SQL Server is now ready for client connections`

5. **Run database migrations**:
   ```bash
   cd HospitalERP.API
   dotnet ef database update
   ```

   If migrations don't exist yet, create one:
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

6. **Run the API** (seed data will be automatically added in Development mode):
   ```bash
   dotnet run
   ```

## Environment Variables

The `.env` file contains:

- `DB_SERVER`: SQL Server hostname (default: localhost)
- `DB_PORT`: SQL Server port (default: 1433)
- `DB_DATABASE`: Database name (default: HospitalERP)
- `DB_USER`: Database user (default: sa)
- `DB_PASSWORD`: Database password (required)
- `KEYCLOAK_AUTHORITY`: Keycloak authority URL
- `KEYCLOAK_AUDIENCE`: Keycloak audience
- `KEYCLOAK_REQUIRE_HTTPS`: Require HTTPS for Keycloak (default: false)

## Docker Commands

### Start services
```bash
docker-compose up -d
```

### Stop services
```bash
docker-compose down
```

### Stop and remove volumes (CAUTION: Deletes all data)
```bash
docker-compose down -v
```

### View logs
```bash
docker-compose logs -f sqlserver
```

### Check container status
```bash
docker-compose ps
```

## Database Connection

Once the container is running, you can connect using:

- **Server**: `localhost,1433`
- **Database**: `HospitalERP`
- **User**: `sa`
- **Password**: (from your `.env` file)

## Seed Data

The application automatically seeds the database with test data when running in Development mode:

- Genders (Male, Female, Other)
- Blood Types (A+, A-, B+, B-, AB+, AB-, O+, O-)
- Roles (Admin, Doctor, Nurse, Receptionist, Pharmacist, Accountant)
- Departments (Emergency, Cardiology, Pediatrics, Orthopedics, Neurology)
- Sample Employees (Admin and Doctors)
- Sample Patients
- Sample Services
- Sample Diagnoses
- Sample Medications with Inventory
- Invoice Types and Payment Statuses

## Troubleshooting

### Container won't start
- Check if port 1433 is already in use: `netstat -an | grep 1433`
- Check Docker logs: `docker-compose logs sqlserver`

### Connection timeout
- Wait for SQL Server to fully initialize (can take 30-60 seconds)
- Check container health: `docker-compose ps`

### Password authentication failed
- Verify `DB_PASSWORD` in `.env` matches what's in `docker-compose.yml`
- SQL Server password must meet complexity requirements (min 8 chars, uppercase, lowercase, number, special char)

## Production Considerations

For production deployment:

1. **Use strong passwords** in `.env` file
2. **Never commit `.env`** to version control (already in `.gitignore`)
3. **Use secrets management** (Azure Key Vault, AWS Secrets Manager, etc.)
4. **Configure SSL/TLS** for database connections
5. **Set up database backups**
6. **Use managed database services** if possible (Azure SQL, AWS RDS)

