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

### 🚧 Partially Implemented Features (DTOs Created, Handlers Needed)

#### **Employees**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllEmployeesQuery, GetEmployeeByIdQuery
- **Commands:** ❌ Need: CreateEmployeeCommand, UpdateEmployeeCommand, DeleteEmployeeCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Departments**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllDepartmentsQuery, GetDepartmentByIdQuery
- **Commands:** ❌ Need: CreateDepartmentCommand, UpdateDepartmentCommand, DeleteDepartmentCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Appointments**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllAppointmentsQuery, GetAppointmentByIdQuery
- **Commands:** ❌ Need: CreateAppointmentCommand, UpdateAppointmentCommand, DeleteAppointmentCommand, UpdateAppointmentStatusCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Medical Records**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllMedicalRecordsQuery, GetMedicalRecordByIdQuery, GetMedicalRecordsByPatientIdQuery
- **Commands:** ❌ Need: CreateMedicalRecordCommand, UpdateMedicalRecordCommand, DeleteMedicalRecordCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Services**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllServicesQuery, GetServiceByIdQuery
- **Commands:** ❌ Need: CreateServiceCommand, UpdateServiceCommand, DeleteServiceCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Medications**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllMedicationsQuery, GetMedicationByIdQuery
- **Commands:** ❌ Need: CreateMedicationCommand, UpdateMedicationCommand, DeleteMedicationCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Inventory**
- **DTOs:** ✅ Complete
- **Queries:** ❌ Need: GetAllInventoryQuery, GetInventoryByIdQuery, GetExpiringInventoryQuery
- **Commands:** ❌ Need: CreateInventoryCommand, UpdateInventoryCommand, AdjustInventoryQuantityCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Invoices**
- **DTOs:** ✅ Complete (includes nested items)
- **Queries:** ❌ Need: GetAllInvoicesQuery, GetInvoiceByIdQuery, GetInvoicesByPatientIdQuery
- **Commands:** ❌ Need: CreateInvoiceCommand (calculate total), UpdateInvoiceCommand, UpdateInvoicePaymentStatusCommand
- **Controller:** ⚠️ Placeholder only
- **AutoMapper:** ❌ Need mappings

#### **Users** (Admin)
- **DTOs:** ❌ Need to create
- **Queries:** ❌ Need to create
- **Commands:** ❌ Need to create
- **Controller:** ⚠️ Placeholder only

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

## Next Steps for Frontend Development

The frontend team can start working with:

1. **Patients API** - Fully implemented and ready to use
2. **DTOs for all features** - All data transfer objects are defined
3. **Database schema** - Complete ERD implemented
4. **API structure** - All endpoints follow consistent patterns

### Recommended Frontend Development Order:
1. ✅ **Patients** - Start here (fully implemented)
2. 🚧 **Departments** - Simple CRUD, good next step
3. 🚧 **Services** - Simple CRUD
4. 🚧 **Employees** - More complex (depends on Departments, Roles)
5. 🚧 **Appointments** - Depends on Patients, Employees, Services
6. 🚧 **Medical Records** - Depends on Patients, Employees, Diagnoses
7. 🚧 **Medications** - Simple CRUD
8. 🚧 **Inventory** - Depends on Medications
9. 🚧 **Invoices** - Complex (nested items, calculations)

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
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This will create all tables with the configured relationships and indexes.

