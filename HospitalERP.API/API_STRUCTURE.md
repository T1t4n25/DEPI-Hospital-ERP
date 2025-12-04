# Hospital ERP API Structure

## Overview

This document describes the API structure that has been created for the Hospital ERP System. The API follows **Vertical Slice Architecture** with **CQRS pattern**.

## Database Schema

### All Entities Created

✅ **Lookup Tables:**

- `Gender` - GenderID, GenderName
- `BloodType` - BloodTypeID, BloodTypeName
- `Role` - RoleID, RoleName
- `InvoiceType` - InvoiceTypeID, InvoiceName
- `PaymentStatus` - PaymentStatusID, StatusName
- `Diagnosis` - DiagnosesID, Diagnoses

✅ **Core Entities:**

- `Patient` - PatientID, FirstName, LastName, DateOfBirth, GenderID, BloodTypeID, Address, ContactNumber
- `Employee` - EmployeeID, FirstName, LastName, GenderID, RoleID, DepartmentID, ContactNumber, HireDate
- `Department` - DepartmentID, DepartmentName, ManagerID
- `EmployeeSchedule` - ScheduleID, EmployeeID, ShiftStart, ShiftEnd

✅ **Medical Records:**

- `MedicalRecord` - RecordID, PatientID, DoctorID, Diagnosesid, DiagnoseDate
- `Treatment` - TreatmentID, DiagnosesID, TreatmentDescription

✅ **Appointments & Services:**

- `Service` - ServiceID, ServiceName, Cost, DepartmentID
- `Appointment` - AppointmentID, PatientID, DoctorID, ServiceID, AppointmentDateTime, Status

✅ **Medications & Inventory:**

- `Medication` - MedicationID, BarCode, Name, Description, Cost
- `Inventory` - MedicationID, Quantity, ExpiryDate

✅ **Invoices & Billing:**

- `Invoice` - InvoiceID, PatientID, InvoiceTypeID, InvoiceDate, TotalAmount, PaymentStatusID, PayDate
- `HospitalInvoiceItem` - InvoiceItemID, InvoiceID, ServiceID, LineTotal
- `MedicationInvoiceItem` - InvoiceItemID, InvoiceID, MedicationID, Quantity, LineTotal

### DbContext Configuration

✅ All DbSets configured with:

- Foreign key relationships
- Delete behaviors (Restrict/Cascade)
- Indexes on searchable fields
- Property constraints (required, max length, precision)

## API Features

### ✅ Fully Implemented Features

#### **Patients** (Complete CRUD)

- **DTOs:** ✅ CreatePatientDto, UpdatePatientDto, PatientListDto, PatientDetailDto
- **Queries:** ✅ GetAllPatientsQuery, GetPatientByIdQuery
- **Commands:** ✅ CreatePatientCommand, UpdatePatientCommand, DeletePatientCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Endpoints:**
  - `GET /api/patients` - Get all patients (paginated)
  - `GET /api/patients/{id}` - Get patient by ID
  - `POST /api/patients` - Create patient (Admin, Receptionist)
  - `PUT /api/patients/{id}` - Update patient (Admin, Receptionist)
  - `DELETE /api/patients/{id}` - Delete patient (Admin only)

### ✅ Fully Implemented Features

#### **Departments** (Complete CRUD)

- **DTOs:** ✅ CreateDepartmentDto, UpdateDepartmentDto, DepartmentListDto, DepartmentDetailDto
- **Queries:** ✅ GetAllDepartmentsQuery, GetDepartmentByIdQuery
- **Commands:** ✅ CreateDepartmentCommand, UpdateDepartmentCommand, DeleteDepartmentCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateDepartmentDtoValidator, UpdateDepartmentDtoValidator
- **Endpoints:**
  - `GET /api/departments` - Get all departments (paginated)
  - `GET /api/departments/{id}` - Get department by ID
  - `POST /api/departments` - Create department (Admin only)
  - `PUT /api/departments/{id}` - Update department (Admin only)
  - `DELETE /api/departments/{id}` - Delete department (Admin only)

#### **Services** (Complete CRUD)

- **DTOs:** ✅ CreateServiceDto, UpdateServiceDto, ServiceListDto, ServiceDetailDto
- **Queries:** ✅ GetAllServicesQuery, GetServiceByIdQuery
- **Commands:** ✅ CreateServiceCommand, UpdateServiceCommand, DeleteServiceCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateServiceDtoValidator, UpdateServiceDtoValidator
- **Endpoints:**
  - `GET /api/services` - Get all services (paginated)
  - `GET /api/services/{id}` - Get service by ID
  - `POST /api/services` - Create service (Admin only)
  - `PUT /api/services/{id}` - Update service (Admin only)
  - `DELETE /api/services/{id}` - Delete service (Admin only)

#### **Employees** (Complete CRUD)

- **DTOs:** ✅ CreateEmployeeDto, UpdateEmployeeDto, EmployeeListDto, EmployeeDetailDto
- **Queries:** ✅ GetAllEmployeesQuery, GetEmployeeByIdQuery
- **Commands:** ✅ CreateEmployeeCommand, UpdateEmployeeCommand, DeleteEmployeeCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateEmployeeDtoValidator, UpdateEmployeeDtoValidator
- **Endpoints:**
  - `GET /api/employees` - Get all employees (paginated)
  - `GET /api/employees/{id}` - Get employee by ID
  - `POST /api/employees` - Create employee (Admin only)
  - `PUT /api/employees/{id}` - Update employee (Admin only)
  - `DELETE /api/employees/{id}` - Delete employee (Admin only)

#### **Appointments** (Complete CRUD)

- **DTOs:** ✅ CreateAppointmentDto, UpdateAppointmentDto, AppointmentListDto, AppointmentDetailDto
- **Queries:** ✅ GetAllAppointmentsQuery, GetAppointmentByIdQuery
- **Commands:** ✅ CreateAppointmentCommand, UpdateAppointmentCommand, DeleteAppointmentCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateAppointmentDtoValidator, UpdateAppointmentDtoValidator
- **Endpoints:**
  - `GET /api/appointments` - Get all appointments (paginated)
  - `GET /api/appointments/{id}` - Get appointment by ID
  - `POST /api/appointments` - Create appointment (Admin, Receptionist)
  - `PUT /api/appointments/{id}` - Update appointment (Admin, Receptionist)
  - `DELETE /api/appointments/{id}` - Delete appointment (Admin only)

#### **Medical Records** (Complete CRUD)

- **DTOs:** ✅ CreateMedicalRecordDto, UpdateMedicalRecordDto, MedicalRecordListDto, MedicalRecordDetailDto
- **Queries:** ✅ GetAllMedicalRecordsQuery, GetMedicalRecordByIdQuery
- **Commands:** ✅ CreateMedicalRecordCommand, UpdateMedicalRecordCommand, DeleteMedicalRecordCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateMedicalRecordDtoValidator, UpdateMedicalRecordDtoValidator
- **Endpoints:**
  - `GET /api/medicalrecords` - Get all medical records (paginated)
  - `GET /api/medicalrecords/{id}` - Get medical record by ID
  - `POST /api/medicalrecords` - Create medical record (Admin, Doctor)
  - `PUT /api/medicalrecords/{id}` - Update medical record (Admin, Doctor)
  - `DELETE /api/medicalrecords/{id}` - Delete medical record (Admin only)

#### **Medications** (Complete CRUD)

- **DTOs:** ✅ CreateMedicationDto, UpdateMedicationDto, MedicationListDto, MedicationDetailDto
- **Queries:** ✅ GetAllMedicationsQuery, GetMedicationByIdQuery
- **Commands:** ✅ CreateMedicationCommand, UpdateMedicationCommand, DeleteMedicationCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateMedicationDtoValidator, UpdateMedicationDtoValidator
- **Endpoints:**
  - `GET /api/medications` - Get all medications (paginated)
  - `GET /api/medications/{id}` - Get medication by ID
  - `POST /api/medications` - Create medication (Admin, Pharmacist)
  - `PUT /api/medications/{id}` - Update medication (Admin, Pharmacist)
  - `DELETE /api/medications/{id}` - Delete medication (Admin only)

#### **Inventory** (Complete CRUD)

- **DTOs:** ✅ CreateInventoryDto, UpdateInventoryDto, InventoryListDto, InventoryDetailDto
- **Queries:** ✅ GetAllInventoryQuery, GetInventoryByIdQuery
- **Commands:** ✅ CreateInventoryCommand, UpdateInventoryCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateInventoryDtoValidator, UpdateInventoryDtoValidator
- **Endpoints:**
  - `GET /api/inventory` - Get all inventory (paginated)
  - `GET /api/inventory/{id}` - Get inventory by ID
  - `POST /api/inventory` - Create inventory (Admin, Pharmacist)
  - `PUT /api/inventory/{id}` - Update inventory (Admin, Pharmacist)

#### **Invoices** (Complete CRUD)

- **DTOs:** ✅ CreateInvoiceDto, UpdateInvoiceDto, InvoiceListDto, InvoiceDetailDto (includes nested items)
- **Queries:** ✅ GetAllInvoicesQuery, GetInvoiceByIdQuery
- **Commands:** ✅ CreateInvoiceCommand, UpdateInvoicePaymentStatusCommand
- **Controller:** ✅ Full CRUD endpoints
- **AutoMapper:** ✅ Complete mappings
- **Validators:** ✅ CreateInvoiceDtoValidator, UpdateInvoiceDtoValidator (includes nested item validators)
- **Endpoints:**
  - `GET /api/invoices` - Get all invoices (paginated)
  - `GET /api/invoices/{id}` - Get invoice by ID
  - `POST /api/invoices` - Create invoice (Admin, Accountant, Receptionist)
  - `PUT /api/invoices/{id}/payment-status` - Update payment status (Admin, Accountant, Receptionist)

#### **Users** (Admin)

- **DTOs:** ⚠️ Placeholder (user management via Keycloak)
- **Controller:** ✅ Placeholder controller
- **AutoMapper:** ✅ UsersProfile (placeholder)

## API Endpoints Structure

All endpoints follow this pattern:

- **Base URL:** `/api/{feature-name}`
- **Authorization:** Keycloak JWT Bearer tokens
- **Roles:** Defined per endpoint (see `.cursorrules`)

### Example: Patients Feature

```
GET    /api/patients?pageNumber=1&pageSize=10&searchTerm=john
GET    /api/patients/{id}
POST   /api/patients
PUT    /api/patients/{id}
DELETE /api/patients/{id}
```

## Common Components

### ✅ Pagination

- `QueryParams` - Reusable pagination parameters
- `PaginatedResponse<T>` - Standard paginated response

### ✅ Exceptions

- `NotFoundException` - For 404 errors
- `BadRequestException` - For 400 errors

### ✅ Validation

- **FluentValidation** - All Create/Update DTOs have validators
- Automatic validation via `AddFluentValidationAutoValidation()`
- Returns 400 Bad Request with detailed error messages for invalid inputs

## Next Steps for Frontend Development

The frontend team can start working with:

1. **All APIs Fully Implemented** - All features have complete CRUD operations
2. **FluentValidation** - All DTOs have validators for data validation
3. **Role-based Authorization** - All endpoints have appropriate role restrictions
4. **Database schema** - Complete ERD implemented with seed data
5. **API structure** - All endpoints follow consistent patterns

### Recommended Frontend Development Order:

1. ✅ **Patients** - Fully implemented with validation
2. ✅ **Departments** - Simple CRUD, good next step
3. ✅ **Services** - Simple CRUD
4. ✅ **Employees** - Complete implementation (depends on Departments, Roles)
5. ✅ **Appointments** - Complete implementation (depends on Patients, Employees, Services)
6. ✅ **Medical Records** - Complete implementation (depends on Patients, Employees, Diagnoses)
7. ✅ **Medications** - Complete implementation with validation
8. ✅ **Inventory** - Complete implementation (depends on Medications)
9. ✅ **Invoices** - Complete implementation (complex with nested items, calculations)

## Testing the API

### Swagger UI

After running the API, access Swagger at:

- **Development:** `https://localhost:5001/swagger` or `http://localhost:5000/swagger`

### Authentication

All endpoints require JWT Bearer token from Keycloak:

- Add `Authorization: Bearer {token}` header to requests
- Configure Keycloak in `appsettings.json`

## Database Migration

To create the database, run:

```bash
cd HospitalERP.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This will create all tables with the configured relationships and indexes.

**Note:** The database is automatically seeded with test data when running in Development mode.
