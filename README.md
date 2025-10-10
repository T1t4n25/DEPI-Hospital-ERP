# 🏥 Hospital ERP System

## 📘 Overview
The **Hospital ERP System** is a web-based application developed using **ASP.NET Core MVC** and **Microsoft SQL Server (MSSQL)**.  
It automates hospital workflows such as patient registration, appointments, billing, employee management, and inventory tracking, ensuring reliability and efficiency for small hospitals.

---

## 🎯 Problem Statement & Objectives

### Problem Statement
Small and medium-sized hospitals often face inefficiencies due to fragmented record-keeping and manual processes.  
Issues include:
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
4. **Enable secure access** with role-based authentication
5. **Provide analytics and reporting**
6. **Use scalable architecture** with ASP.NET Core MVC + MSSQL

---

## 👥 Stakeholders & System Actors

| Actor | Responsibility |
|--------|----------------|
| **Admin** | Manages users, roles, departments, and reports |
| **Doctor** | Views patient records, adds diagnoses, and manages treatments |
| **Receptionist** | Registers patients, schedules appointments, and issues invoices |
| **Pharmacist** | Manages medication inventory and prescriptions |
| **Accountant** | Handles invoices, payments, and financial reports |
| **Patient** | (Optional) Views appointments or payment history via a portal |

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
| Module | Requirement |
|---------|-------------|
| Patient Management | CRUD operations with linked records |
| Appointments | Scheduling, updating, canceling |
| Medical Records | Diagnoses and treatments per patient |
| Billing | Automated total calculation and payment tracking |
| Inventory | Medication management with alerts |
| HR | Role and schedule management |
| Reports | Summary of operations and financials |

### Non-Functional Requirements
| Category | Description |
|-----------|-------------|
| **Performance** | ≤ 2s response time, ≤ 100 concurrent users |
| **Scalability** | Modular design for future modules |
| **Security** | ASP.NET Identity with role-based authorization |
| **Data Integrity** | Enforced via MSSQL foreign keys |
| **Availability** | ≥ 99% uptime during working hours |
| **Maintainability** | Clear MVC separation for maintainable code |
| **Usability** | Clean UI using Razor Pages and Bootstrap |
| **Backup & Recovery** | Automated MSSQL daily backups |
| **Compatibility** | Compatible with modern browsers and Windows Server |

---

## 🏗️ Software Architecture

### Architecture Overview
Built on the **Model–View–Controller (MVC)** pattern in **.NET Core 8.0**, ensuring clean separation between UI, business logic, and data.

### Component Breakdown

| Layer | Description | Technology |
|--------|-------------|-------------|
| **Presentation Layer (View)** | Razor Pages + Bootstrap-based UI | HTML, CSS, Bootstrap |
| **Controller Layer** | Handles requests, logic, and routing | ASP.NET Core Controllers |
| **Model Layer** | Entity models and EF Core ORM | Entity Framework Core |
| **Database Layer** | Stores relational data | Microsoft SQL Server |
| **Authentication** | Login, roles, permissions | ASP.NET Core Identity |
| **Reporting** | Analytics and reports | RDLC / FastReport.NET / Razor Reports |

---

## 🧱 Technology Stack

**Backend:** ASP.NET Core 8.0 MVC, EF Core  
**Frontend:** Razor Pages, HTML5, CSS3, Bootstrap  
**Database:** Microsoft SQL Server (MSSQL)  
**Authentication:** ASP.NET Identity  
**Reporting:** RDLC / FastReport.NET  
**Hosting:** IIS (on-premise) or Azure App Service  

---

## 🗓️ Gantt Chart (10-Week Project Plan)

| Week | Task | Start Date | End Date | Deliverables |
|------|------|-------------|-----------|---------------|
| 1 | Requirements & DB Design | 2025-10-06 | 2025-10-12 | ERD + Project Setup |
| 2 | Core Models & CRUD | 2025-10-13 | 2025-10-19 | Patient & Employee CRUD |
| 3 | Authentication & Roles | 2025-10-20 | 2025-10-26 | Login & Role System |
| 4 | Appointments Module | 2025-10-27 | 2025-11-02 | Appointments Scheduling |
| 5 | Medical Records | 2025-11-03 | 2025-11-09 | Medical Records UI |
| 6 | Billing & Invoices | 2025-11-10 | 2025-11-16 | Billing Module |
| 7 | Billing & Invoices (cont.) | 2025-11-17 | 2025-11-23 | Invoices Reports |
| 8 | Inventory & Medications | 2025-11-24 | 2025-11-30 | Inventory & Expiry Alerts |
| 9 | Testing & QA | 2025-12-01 | 2025-12-07 | QA & Bug Fixes |
| 10 | Deployment & Documentation | 2025-12-08 | 2025-12-14 | Deployed System + Docs |

---

## 👨‍💻 Team & Task Assignment

| Role | Responsibility |
|------|----------------|
| **Backend Dev 1** | Database design, Entity Framework, core logic |
| **Backend Dev 2** | Billing, Invoices, Medications |
| **Frontend Dev 1** | Razor Views for Patients & Appointments |
| **Frontend Dev 2** | Billing & Inventory UI, Validation, Testing |

---

## ⚠️ Risk Assessment & Mitigation

| Risk | Mitigation |
|------|-------------|
| DB schema changes mid-project | Lock schema after Week 2 |
| Controller/View mismatch | Use shared ViewModels |
| Authentication issues | Test early with ASP.NET Identity |
| Deployment errors | Test on IIS in advance |

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
- ASP.NET MVC Source Code (C#)
- SQL Server Database + Scripts
- Documentation (Word/PDF + README)
- Test & Deployment Reports
- Presentation Slides

---

## 📈 Deployment & Hosting
- **Framework:** .NET Core MVC 8.0  
- **Database:** Microsoft SQL Server  
- **Hosting:** IIS / Azure App Service  
- **ORM:** Entity Framework Core  
- **Auth:** ASP.NET Identity  
- **Reporting:** RDLC / FastReport.NET  

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
        
    - `ContactNumber` (VARCHAR)
        
- **MedicalRecords:**
    
    - `RecordID` (Primary Key, INT)
        
    - `PatientID` (Foreign Key to `Patients`, INT)
	    
    - `DoctorID` (FK to `Employees`, INT)
	    
	- `Diagnosesid` (FK to Diagnoses, INT)
        
    - `DiagnoseDate` (DATE)
	
-  **Diagnoses**
	
	- `DiagnosesID` (PK, INT)
		
	- `Diagnoses` (VARCHAR)
    
-  **Treatment**
	
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
