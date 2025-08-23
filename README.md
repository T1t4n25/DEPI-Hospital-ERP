
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
