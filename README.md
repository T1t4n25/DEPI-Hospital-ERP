# DEPI-Hospital-ERP

# Database ERD

### Patient Management

- **Patients:**
    
    - `PatientID` (Primary Key, INT)
        
    - `FirstName` (VARCHAR)
        
    - `LastName` (VARCHAR)
        
    - `DateOfBirth` (DATE)
        
    - `Gender` (VARCHAR)
        
    - `Address` (VARCHAR)
        
    - `ContactNumber` (VARCHAR)
        
- **MedicalRecords:**
    
    - `RecordID` (Primary Key, INT)
        
    - `PatientID` (Foreign Key to `Patients`, INT)
	    
    - Diagnosis (VARCHAR)
        
    - `DiagnoseData` (VARCHAR)
        
- **Appointments:**
    
    - `AppointmentID` (Primary Key, INT)
        
    - `PatientID` (Foreign Key to `Patients`, INT)
        
    - `DoctorID` (Foreign Key to `Employees`, INT)
        
    - `AppointmentDateTime` (DATETIME)
        
    - `Status` (VARCHAR)
        

---

### Billing

- **HospitalInvoices:**
    
    - `InvoiceID` (Primary Key, INT)
        
    - `PatientID` (Foreign Key to `Patients`, INT)
        
    - `InvoiceDate` (DATE)
        
    - `TotalAmount` (DECIMAL)
        
    - `PaymentStatus` (VARCHAR)
        
- **HospitalInvoiceItems:**
    
    - `InvoiceItemID` (Primary Key, INT)
        
    - `InvoiceID` (Foreign Key to `HospitalInvoices`, INT)
        
    - `ServiceID` (Foreign Key to `Services`, INT)
        
    - `LineTotal` (DECIMAL)
        
- **MedicationInvoices:**
    
    - `InvoiceID` (Primary Key, INT)
        
    - `PatientID` (Foreign Key to `Patients`, INT)
        
    - `InvoiceDate` (DATE)
        
    - `TotalAmount` (DECIMAL)
        
    - `PaymentStatus` (VARCHAR)
        
- **MedicationInvoiceItems:**
    
    - `InvoiceItemID` (Primary Key, INT)
        
    - `InvoiceID` (Foreign Key to `MedicationInvoices`, INT)
        
    - `MedicationID` (Foreign Key to `Medications`, INT)
        
    - `Quantity` (INT)
        
    - `LineTotal` (DECIMAL)
        

---

### Human Resources and Reporting

- **Departments:**
    
    - `DepartmentID` (Primary Key, INT)
        
    - `DepartmentName` (VARCHAR)
        
- **Roles:**
    
    - `RoleID` (Primary Key, INT)
        
    - `RoleName` (VARCHAR)
        
- **Employees:**
    
    - `EmployeeID` (Primary Key, INT)
        
    - `FirstName` (VARCHAR)
        
    - `LastName` (VARCHAR)
        
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
        
    - `Name` (VARCHAR)
        
    - `Description` (VARCHAR)
	    
    - Cost (DECIMAL)
        
    - `Dosage` (VARCHAR)
