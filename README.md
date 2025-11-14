# 🏥 Hospital ERP System

## 📘 Overview

The **Hospital ERP System** is a modern web-based application developed using **.NET Core Web API** (backend) and **Angular** (frontend) with **Microsoft SQL Server (MSSQL)**.
It automates hospital workflows such as patient registration, appointments, billing, employee management, and inventory tracking, ensuring reliability and efficiency for small hospitals.

---

## 🎯 Problem Statement & Objectives

### Problem Statement

Small and medium-sized hospitals often face inefficiencies due to fragmented record-keeping and manual processes.Issues include:

- Redundant patient records
- Appointment scheduling conflicts
- Billing and payment errors
- Poor visibility into employee roles and schedules
- Weak inventory control

### Objectives

The system aims to:

1. **Centralize hospital operations** (patients, billing, HR, inventory)
2. **Automate scheduling and invoicing**
3. **Ensure data integrity** through MSSQL foreign keys
4. **Enable secure access** with role-based authentication via Keycloak
5. **Provide analytics and reporting**
6. **Use scalable architecture** with .NET Core Web API + Angular + MSSQL

---

## 👥 Stakeholders & System Actors

| Actor                  | Responsibility                                                  |
| ---------------------- | --------------------------------------------------------------- |
| **Admin**        | Manages users, roles, departments, and reports                  |
| **Doctor**       | Views patient records, adds diagnoses, and manages treatments   |
| **Receptionist** | Registers patients, schedules appointments, and issues invoices |
| **Pharmacist**   | Manages medication inventory and prescriptions                  |
| **Accountant**   | Handles invoices, payments, and financial reports               |
| **Patient**      | (Optional) Views appointments or payment history via a portal   |

---

## 🧩 Functional Modules

- **Patient Management** — Manage patient data and medical history
- **Appointment Scheduling** — Link patients with doctors and services
- **Billing & Invoicing** — Generate invoices for treatments and medications
- **Inventory Management** — Track medication quantities and expiry dates
- **Human Resources** — Manage employees, roles, and departments
- **Reports & Analytics** — Generate insights and financial summaries

---

## ⚙️ Functional & Non-Functional Requirements

### Functional Requirements

| Module             | Requirement                                      |
| ------------------ | ------------------------------------------------ |
| Patient Management | CRUD operations with linked records              |
| Appointments       | Scheduling, updating, canceling                  |
| Medical Records    | Diagnoses and treatments per patient             |
| Billing            | Automated total calculation and payment tracking |
| Inventory          | Medication management with alerts                |
| HR                 | Role and schedule management                     |
| Reports            | Summary of operations and financials             |

### Non-Functional Requirements

| Category                    | Description                                                         |
| --------------------------- | ------------------------------------------------------------------- |
| **Performance**       | ≤ 2s response time, ≤ 100 concurrent users                        |
| **Scalability**       | Modular design with Vertical Slice Architecture for future modules  |
| **Security**          | Keycloak JWT with role-based authorization                          |
| **Data Integrity**    | Enforced via MSSQL foreign keys                                     |
| **Availability**      | ≥ 99% uptime during working hours                                  |
| **Maintainability**   | Vertical Slice Architecture with CQRS pattern for maintainable code |
| **Usability**         | Modern UI using Angular with responsive design                      |
| **Backup & Recovery** | Automated MSSQL daily backups                                       |
| **Compatibility**     | Compatible with modern browsers (Chrome, Firefox, Edge, Safari)     |

---

## 🏗️ Software Architecture

### Architecture Overview

Built on **Vertical Slice Architecture** with **CQRS (Command Query Responsibility Segregation)** pattern in **.NET 9 Web API** for the backend and **Angular** for the frontend, ensuring clean separation between UI, business logic, and data.

### Component Breakdown

| Layer                          | Description                          | Technology                              |
| ------------------------------ | ------------------------------------ | --------------------------------------- |
| **Frontend Layer**       | Angular SPA with responsive UI       | Angular, TypeScript, HTML5, CSS3        |
| **API Layer**            | RESTful Web API with Vertical Slices | .NET 9 Web API, MediatR                 |
| **Business Logic Layer** | Commands and Queries (CQRS)          | C# Handlers, AutoMapper                 |
| **Data Access Layer**    | Entity models and EF Core ORM        | Entity Framework Core                   |
| **Database Layer**       | Stores relational data               | Microsoft SQL Server                    |
| **Authentication**       | JWT-based authentication with roles  | Keycloak                                |
| **Reporting**            | Analytics and reports                | RDLC / FastReport.NET / Angular Reports |

---

## 🧱 Technology Stack

**Backend:**

- .NET 9 Web API
- Entity Framework Core
- MediatR (CQRS pattern)
- AutoMapper
- FluentValidation
- Vertical Slice Architecture

**Frontend:**

- Angular (TypeScript)
- HTML5, CSS3
- Angular Material / Bootstrap (responsive UI)

**Database:**

- Microsoft SQL Server (MSSQL)

**Authentication & Authorization:**

- Keycloak (JWT tokens)
- Role-based access control (RBAC)

**Reporting:**

- RDLC / FastReport.NET

**Hosting:**

- Production: VPS with Docker & Docker Compose
- Reverse Proxy: Nginx
- SSL/TLS: Let's Encrypt (Certbot)
- Containerization: Docker containers for all services

---

## 🗓️ Gantt Chart (10-Week Project Plan)

| Week | Task                       | Start Date | End Date   | Deliverables              |
| ---- | -------------------------- | ---------- | ---------- | ------------------------- |
| 1    | Requirements & DB Design   | 2025-10-06 | 2025-10-12 | ERD + Project Setup       |
| 2    | Core Models & CRUD         | 2025-10-13 | 2025-10-19 | Patient & Employee CRUD   |
| 3    | Authentication & Roles     | 2025-10-20 | 2025-10-26 | Login & Role System       |
| 4    | Appointments Module        | 2025-10-27 | 2025-11-02 | Appointments Scheduling   |
| 5    | Medical Records            | 2025-11-03 | 2025-11-09 | Medical Records UI        |
| 6    | Billing & Invoices         | 2025-11-10 | 2025-11-16 | Billing Module            |
| 7    | Billing & Invoices (cont.) | 2025-11-17 | 2025-11-23 | Invoices Reports          |
| 8    | Inventory & Medications    | 2025-11-24 | 2025-11-30 | Inventory & Expiry Alerts |
| 9    | Testing & QA               | 2025-12-01 | 2025-12-07 | QA & Bug Fixes            |
| 10   | Deployment & Documentation | 2025-12-08 | 2025-12-14 | Deployed System + Docs    |

---

## 👨‍💻 Team & Task Assignment

| Role                     | Responsibility                                                  |
| ------------------------ | --------------------------------------------------------------- |
| **Backend Dev 1**  | Database design, Entity Framework, API endpoints, CQRS handlers |
| **Backend Dev 2**  | Billing, Invoices, Medications, Keycloak integration            |
| **Frontend Dev 1** | Angular components for Patients & Appointments                  |
| **Frontend Dev 2** | Billing & Inventory UI, Angular services, Testing               |

---

## ⚠️ Risk Assessment & Mitigation

| Risk                            | Mitigation                                      |
| ------------------------------- | ----------------------------------------------- |
| DB schema changes mid-project   | Lock schema after Week 2                        |
| API/Frontend integration issues | Use TypeScript interfaces and OpenAPI/Swagger   |
| Authentication issues           | Test early with Keycloak integration            |
| CORS issues                     | Configure CORS properly in API startup          |
| Docker container issues         | Test Docker setup locally before VPS deployment |
| VPS resource constraints        | Monitor container resources, optimize images    |
| SSL certificate expiration      | Set up automatic renewal with Certbot           |
| Deployment errors               | Test Docker Compose on staging VPS in advance   |

---

## 📊 Key Performance Indicators (KPIs)

- Response time ≤ **2s**
- System uptime ≥ **99%**
- Billing accuracy ≥ **98%**
- Registration time ≤ **2 min**
- Test coverage ≥ **80%**
- On-time delivery ≥ **90%**

---

## 📦 Deliverables

- .NET Core Web API Source Code (C#) with Vertical Slice Architecture
- Angular Frontend Source Code (TypeScript)
- SQL Server Database + Scripts
- Keycloak Configuration
- Docker Configuration Files (Dockerfile, docker-compose.yml)
- Nginx Configuration Files
- Documentation (Word/PDF + README)
- Test & Deployment Reports
- Presentation Slides

---

## 📈 Deployment & Hosting

### Production Environment

- **Hosting:** VPS (Virtual Private Server) with Docker
- **Orchestration:** Docker Compose
- **Reverse Proxy:** Nginx
- **SSL/TLS:** Let's Encrypt (via Certbot)
- **Container Management:** Docker & Docker Compose

### Backend API

- **Framework:** .NET 9 Web API
- **Database:** Microsoft SQL Server (Docker container)
- **Container:** Docker containerized .NET API
- **ORM:** Entity Framework Core
- **Auth:** Keycloak (JWT) - Docker container
- **Architecture:** Vertical Slice Architecture with CQRS
- **Port:** Exposed via Nginx reverse proxy (HTTPS)

### Frontend Application

- **Framework:** Angular
- **Build:** Angular CLI (ng build --prod)
- **Hosting:** Nginx serving static files (Docker container)
- **API Communication:** HTTP Client with JWT interceptors
- **Port:** Exposed via Nginx reverse proxy (HTTPS)

### Docker Services

The application runs as a multi-container Docker setup:

- **API Container:** .NET 9 Web API
- **Frontend Container:** Nginx serving Angular build
- **Database Container:** SQL Server
- **Keycloak Container:** Authentication server
- **Nginx Container:** Reverse proxy and load balancer

### Development Setup

- **API URL:** `http://localhost:5000` (or configured port)
- **Frontend URL:** `http://localhost:4200` (Angular default)
- **Keycloak URL:** `http://localhost:8080` (configured during setup)

---

## 🚀 Getting Started

### Prerequisites

- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Node.js** (v18 or higher) and **npm** - [Download](https://nodejs.org/)
- **Angular CLI** - Install via `npm install -g @angular/cli`
- **SQL Server** - SQL Server 2019 or higher / SQL Server Express
- **Keycloak** - For authentication (can run via Docker or standalone)

### Backend Setup (.NET Core API)

1. **Navigate to the backend directory:**

   ```bash
   cd backend
   ```
2. **Restore dependencies:**

   ```bash
   dotnet restore
   ```
3. **Configure connection string:**

   - Update `appsettings.json` with your SQL Server connection string
   - Update Keycloak configuration in `appsettings.json`
4. **Run database migrations:**

   ```bash
   dotnet ef database update
   ```
5. **Run the API:**

   ```bash
   dotnet run
   ```

   - API will be available at `http://localhost:5000` (or configured port)

### Frontend Setup (Angular)

1. **Navigate to the frontend directory:**

   ```bash
   cd frontend
   ```
2. **Install dependencies:**

   ```bash
   npm install
   ```
3. **Configure API endpoint:**

   - Update `environment.ts` with your API URL:
     ```typescript
     export const environment = {
       production: false,
       apiUrl: 'http://localhost:5000/api'
     };
     ```
4. **Run the Angular development server:**

   ```bash
   ng serve
   ```

   - Frontend will be available at `http://localhost:4200`

### Authentication Setup (Keycloak)

1. **Configure Keycloak realm** with the following roles:

   - Admin
   - Doctor
   - Receptionist
   - Pharmacist
   - Accountant
2. **Update API `appsettings.json`** with Keycloak configuration:

   ```json
   "Keycloak": {
     "Authority": "http://localhost:8080/auth/realms/your-realm",
     "Audience": "your-client-id"
   }
   ```
3. **Update Angular `environment.ts`** with Keycloak client configuration

### Running the Application (Development)

1. **Start Keycloak** (if using local instance)
2. **Start the .NET API** (backend)
3. **Start the Angular application** (frontend)
4. **Access the application** at `http://localhost:4200`
5. **Login** using credentials configured in Keycloak

---

## 🐳 Docker Deployment (Production - VPS)

### Prerequisites for VPS

- **VPS with Ubuntu/Debian** (recommended: 4GB RAM, 2 CPU cores minimum)
- **Docker** installed on VPS
- **Docker Compose** installed on VPS
- **Domain name** (optional, for SSL)

### VPS Setup Steps

1. **SSH into your VPS:**

   ```bash
   ssh user@your-vps-ip
   ```
2. **Install Docker & Docker Compose** (if not installed):

   ```bash
   # Install Docker
   curl -fsSL https://get.docker.com -o get-docker.sh
   sudo sh get-docker.sh

   # Install Docker Compose
   sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
   sudo chmod +x /usr/local/bin/docker-compose
   ```
3. **Clone the repository:**

   ```bash
   git clone <your-repo-url>
   cd "Final Project"
   ```
4. **Configure environment variables:**

   - Update `docker-compose.yml` with production settings
   - Update `.env` file with database credentials, Keycloak settings, etc.
5. **Build and run with Docker Compose:**

   ```bash
   docker-compose -f docker-compose.prod.yml up -d --build
   ```

### Docker Compose Structure

The `docker-compose.prod.yml` file should include:

- **API service:** .NET 9 Web API container
- **Frontend service:** Nginx serving Angular build
- **Database service:** SQL Server container
- **Keycloak service:** Keycloak authentication server
- **Nginx service:** Reverse proxy for all services

### Nginx Configuration

Nginx acts as a reverse proxy:

- Routes `/api/*` to the .NET API container
- Serves Angular static files for all other routes
- Handles SSL/TLS termination
- Configures CORS headers

### SSL/TLS Setup (Optional)

1. **Install Certbot:**

   ```bash
   sudo apt-get update
   sudo apt-get install certbot python3-certbot-nginx
   ```
2. **Obtain SSL certificate:**

   ```bash
   sudo certbot --nginx -d yourdomain.com
   ```
3. **Auto-renewal:**

   ```bash
   sudo certbot renew --dry-run
   ```

### Docker Commands

```bash
# Start all services
docker-compose up -d

# Stop all services
docker-compose down

# View logs
docker-compose logs -f

# Restart a specific service
docker-compose restart api

# Rebuild and restart
docker-compose up -d --build
```

### Production Environment Variables

Ensure the following are configured in `.env` or `docker-compose.yml`:

- Database connection strings
- Keycloak URLs and credentials
- API base URLs
- CORS allowed origins
- JWT secret keys

---

## 📜 License

This project is for academic and internal institutional use.
© 2025 Hospital ERP Development Team.

---

# ERD Written

### Patient Management

- **Patients:**

  - `PatientID` (Primary Key, INT)
  - `FirstName` (VARCHAR)
  - `LastName` (VARCHAR)
  - `DateOfBirth` (DATE)
  - `Gender` (FK to `Gender`, INT)
  - `Address` (VARCHAR)
  - `bloodType` (fk to `BloodType`, INT)
  - `ContactNumber` (VARCHAR)
- **MedicalRecords:**

  - `RecordID` (Primary Key, INT)
  - `PatientID` (Foreign Key to `Patients`, INT)
  - `DoctorID` (FK to `Employees`, INT)
  - `Diagnosesid` (FK to Diagnoses, INT)
  - `DiagnoseDate` (DATE)
- **Diagnoses**

  - `DiagnosesID` (PK, INT)
  - `Diagnoses` (VARCHAR)
- **Treatment**

  - `TreatmentID` (PK, INT)
  - `DiagnosesID` (FK to `Diagnoses`, INT)
  - `Treatment` (VARCHAR)
- **Appointments:**

  - `AppointmentID` (Primary Key, INT)
  - `PatientID` (Foreign Key to `Patients`, INT)
  - `DoctorID` (Foreign Key to `Employees`, INT)
  - `ServiceID` (FK to `Services`, INT)
  - `AppointmentDateTime` (DATETIME)
  - `Status` (VARCHAR)

---

### Billing

- **Invoices:**

  - `InvoiceID` (Primary Key, INT)
  - `PatientID` (Foreign Key to `Patients`, INT)
  - `InvoiceType` (FK to `InvoiceType`, INT)
  - `InvoiceDate` (DATE)
  - `TotalAmount` (Derived from `LineTotal`, DECIMAL)
  - `PaymentStatus` (FK to `PaymentStatus`, INT)
  - `PayDate` (DATE)
- **HospitalInvoiceItems:**

  - `InvoiceItemID` (Primary Key, INT)
  - `InvoiceID` (Foreign Key to `Invoices`, INT)
  - `ServiceID` (Foreign Key to `Services`, INT)
  - `LineTotal` (DECIMAL)
- **MedicationInvoiceItems:**

  - `InvoiceItemID` (Primary Key, INT)
  - `InvoiceID` (Foreign Key to `Invoices`, INT)
  - `MedicationID` (Foreign Key to `Medications`, INT)
  - `Quantity` (INT)
  - `LineTotal` (DECIMAL)

---

### Human Resources and Reporting

- **Departments:**

  - `DepartmentID` (Primary Key, INT)
  - `DepartmentName` (VARCHAR)
  - `Manager` (Foreign Key to `Employees`, INT)
- **Roles:**

  - `RoleID` (Primary Key, INT)
  - `RoleName` (VARCHAR)
- **Employees:**

  - `EmployeeID` (Primary Key, INT)
  - `FirstName` (VARCHAR)
  - `LastName` (VARCHAR)
  - `Gender` (FK to Gender, INT)
  - `RoleID` (Foreign Key to `Roles`, INT)
  - `DepartmentID` (Foreign Key to `Departments`, INT)
  - `ContactNumber` (VARCHAR)
  - `HireDate` (DATE)
- **EmployeeSchedules:**

  - `ScheduleID` (Primary Key, INT)
  - `EmployeeID` (Foreign Key to `Employees`, INT)
  - `ShiftStart` (TIME)
  - `ShiftEnd` (TIME)

---

### Services and Medication

- **Services:**

  - `ServiceID` (Primary Key, INT)
  - `ServiceName` (VARCHAR)
  - `Cost` (DECIMAL)
  - `DepartmentID` (Foreign Key to `Departments`, INT)
- **Medications:**

  - `MedicationID` (Primary Key, INT)
  - `BarCode` (VARCHAR(13))
  - `Name` (Primary Key, VARCHAR)
  - `Description` (VARCHAR)
  - `Cost` (DECIMAL)
- **Inventory:**

  - `MedicationID` (Primary Key, INT)
  - `Quantity` (INT)
  - `ExpiryDate` (DATE)

# Lookup Tables for Normalization

- **Gender:**

  - `GenderID` (Primary Key, INT)
  - `GenderName` (VARCHAR)
- **PaymentStatus:**

  - `PaymentStatusID` (Primary Key, INT)
  - `StatusName` (VARCHAR)
- `InvoiceType:`

  - `InvoiceTypeID` (Primary Key, INT)
  - `InvoiceName` (VARCHAR)

  `BloodType:`

  - `BloodTypeID` (Primary Key, INT)
  - `BloodTypeName` (VARCHAR)
