using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Features.Appointments.Dtos;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Features.Employees.Dtos;
using HospitalERP.API.Features.Inventory.Dtos;
using HospitalERP.API.Features.Invoices.Dtos;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using HospitalERP.API.Features.Medications.Dtos;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Features.Services.Dtos;

namespace HospitalERP.API.Common.Swagger;

public static class ExamplesProvider
{
    // Patients
    public static PatientDetailDto PatientDetailExample => new()
    {
        PatientID = 1,
        FirstName = "John",
        LastName = "Doe",
        DateOfBirth = new DateOnly(1985, 5, 15),
        GenderID = 1,
        GenderName = "Male",
        Address = "123 Main Street, City, State 12345",
        BloodTypeID = 1,
        BloodTypeName = "O+",
        ContactNumber = "+1-555-0123"
    };

    public static PaginatedResponse<PatientListDto> PatientsPaginatedExample => new(
        new List<PatientListDto>
        {
            new PatientListDto
            {
                PatientID = 1,
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateOnly(1985, 5, 15),
                GenderName = "Male",
                ContactNumber = "+1-555-0123"
            },
            new PatientListDto
            {
                PatientID = 2,
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateOnly(1990, 8, 22),
                GenderName = "Female",
                ContactNumber = "+1-555-0124"
            }
        },
        1, 10, 2
    );

    // Departments
    public static DepartmentDetailDto DepartmentDetailExample => new()
    {
        DepartmentID = 1,
        DepartmentName = "Cardiology",
        ManagerID = 1,
        ManagerName = "Dr. Sarah Johnson"
    };

    public static PaginatedResponse<DepartmentListDto> DepartmentsPaginatedExample => new(
        new List<DepartmentListDto>
        {
            new DepartmentListDto
            {
                DepartmentID = 1,
                DepartmentName = "Cardiology",
                ManagerName = "Dr. Sarah Johnson"
            },
            new DepartmentListDto
            {
                DepartmentID = 2,
                DepartmentName = "Pediatrics",
                ManagerName = "Dr. Michael Brown"
            }
        },
        1, 10, 2
    );

    // Services
    public static ServiceDetailDto ServiceDetailExample => new()
    {
        ServiceID = 1,
        ServiceName = "ECG (Electrocardiogram)",
        Cost = 150.00m,
        DepartmentID = 1,
        DepartmentName = "Cardiology"
    };

    public static PaginatedResponse<ServiceListDto> ServicesPaginatedExample => new(
        new List<ServiceListDto>
        {
            new ServiceListDto
            {
                ServiceID = 1,
                ServiceName = "ECG (Electrocardiogram)",
                Cost = 150.00m,
                DepartmentName = "Cardiology"
            },
            new ServiceListDto
            {
                ServiceID = 2,
                ServiceName = "Blood Test",
                Cost = 75.50m,
                DepartmentName = "Laboratory"
            }
        },
        1, 10, 2
    );

    // Employees
    public static EmployeeDetailDto EmployeeDetailExample => new()
    {
        EmployeeID = 1,
        FirstName = "Sarah",
        LastName = "Johnson",
        GenderID = 2,
        GenderName = "Female",
        RoleID = 1,
        RoleName = "Doctor",
        DepartmentID = 1,
        DepartmentName = "Cardiology",
        ContactNumber = "+1-555-0200",
        HireDate = new DateOnly(2020, 1, 15)
    };

    public static PaginatedResponse<EmployeeListDto> EmployeesPaginatedExample => new(
        new List<EmployeeListDto>
        {
            new EmployeeListDto
            {
                EmployeeID = 1,
                FirstName = "Sarah",
                LastName = "Johnson",
                RoleName = "Doctor",
                DepartmentName = "Cardiology",
                ContactNumber = "+1-555-0200"
            },
            new EmployeeListDto
            {
                EmployeeID = 2,
                FirstName = "Michael",
                LastName = "Brown",
                RoleName = "Doctor",
                DepartmentName = "Pediatrics",
                ContactNumber = "+1-555-0201"
            }
        },
        1, 10, 2
    );

    // Appointments
    public static AppointmentDetailDto AppointmentDetailExample => new()
    {
        AppointmentID = 1,
        PatientID = 1,
        PatientName = "John Doe",
        DoctorID = 1,
        DoctorName = "Dr. Sarah Johnson",
        ServiceID = 1,
        ServiceName = "ECG (Electrocardiogram)",
        AppointmentDateTime = new DateTime(2024, 12, 15, 10, 30, 0),
        Status = "Scheduled"
    };

    public static PaginatedResponse<AppointmentListDto> AppointmentsPaginatedExample => new(
        new List<AppointmentListDto>
        {
            new AppointmentListDto
            {
                AppointmentID = 1,
                PatientName = "John Doe",
                DoctorName = "Dr. Sarah Johnson",
                ServiceName = "ECG (Electrocardiogram)",
                AppointmentDateTime = new DateTime(2024, 12, 15, 10, 30, 0),
                Status = "Scheduled"
            },
            new AppointmentListDto
            {
                AppointmentID = 2,
                PatientName = "Jane Smith",
                DoctorName = "Dr. Michael Brown",
                ServiceName = "Blood Test",
                AppointmentDateTime = new DateTime(2024, 12, 16, 14, 0, 0),
                Status = "Scheduled"
            }
        },
        1, 10, 2
    );

    // Medical Records
    public static MedicalRecordDetailDto MedicalRecordDetailExample => new()
    {
        RecordID = 1,
        PatientID = 1,
        PatientName = "John Doe",
        DoctorID = 1,
        DoctorName = "Dr. Sarah Johnson",
        Diagnosesid = 1,
        Diagnosis = "Hypertension",
        DiagnoseDate = new DateOnly(2024, 12, 10)
    };

    public static PaginatedResponse<MedicalRecordListDto> MedicalRecordsPaginatedExample => new(
        new List<MedicalRecordListDto>
        {
            new MedicalRecordListDto
            {
                RecordID = 1,
                PatientName = "John Doe",
                DoctorName = "Dr. Sarah Johnson",
                Diagnosis = "Hypertension",
                DiagnoseDate = new DateOnly(2024, 12, 10)
            },
            new MedicalRecordListDto
            {
                RecordID = 2,
                PatientName = "Jane Smith",
                DoctorName = "Dr. Michael Brown",
                Diagnosis = "Common Cold",
                DiagnoseDate = new DateOnly(2024, 12, 11)
            }
        },
        1, 10, 2
    );

    // Medications
    public static MedicationDetailDto MedicationDetailExample => new()
    {
        MedicationID = 1,
        BarCode = "1234567890123",
        Name = "Paracetamol 500mg",
        Description = "Pain reliever and fever reducer",
        Cost = 5.99m,
        Quantity = 100,
        ExpiryDate = new DateOnly(2026, 12, 31)
    };

    public static PaginatedResponse<MedicationListDto> MedicationsPaginatedExample => new(
        new List<MedicationListDto>
        {
            new MedicationListDto
            {
                MedicationID = 1,
                BarCode = "1234567890123",
                Name = "Paracetamol 500mg",
                Cost = 5.99m,
                Quantity = 100
            },
            new MedicationListDto
            {
                MedicationID = 2,
                BarCode = "9876543210987",
                Name = "Ibuprofen 400mg",
                Cost = 7.50m,
                Quantity = 75
            }
        },
        1, 10, 2
    );

    // Inventory
    public static InventoryDetailDto InventoryDetailExample => new()
    {
        MedicationID = 1,
        MedicationName = "Paracetamol 500mg",
        BarCode = "1234567890123",
        Description = "Pain reliever and fever reducer",
        Cost = 5.99m,
        Quantity = 100,
        ExpiryDate = new DateOnly(2026, 12, 31)
    };

    public static PaginatedResponse<InventoryListDto> InventoryPaginatedExample => new(
        new List<InventoryListDto>
        {
            new InventoryListDto
            {
                MedicationID = 1,
                MedicationName = "Paracetamol 500mg",
                BarCode = "1234567890123",
                Quantity = 100,
                ExpiryDate = new DateOnly(2026, 12, 31)
            },
            new InventoryListDto
            {
                MedicationID = 2,
                MedicationName = "Ibuprofen 400mg",
                BarCode = "9876543210987",
                Quantity = 75,
                ExpiryDate = new DateOnly(2027, 6, 30)
            }
        },
        1, 10, 2
    );

    // Invoices
    public static InvoiceDetailDto InvoiceDetailExample => new()
    {
        InvoiceID = 1,
        PatientID = 1,
        PatientName = "John Doe",
        InvoiceTypeID = 1,
        InvoiceTypeName = "Service",
        InvoiceDate = new DateOnly(2024, 12, 15),
        TotalAmount = 225.50m,
        PaymentStatusID = 1,
        PaymentStatusName = "Pending",
        PayDate = null,
        HospitalItems = new List<HospitalInvoiceItemDto>
        {
            new HospitalInvoiceItemDto
            {
                InvoiceItemID = 1,
                ServiceID = 1,
                ServiceName = "ECG (Electrocardiogram)",
                LineTotal = 150.00m
            }
        },
        MedicationItems = new List<MedicationInvoiceItemDto>
        {
            new MedicationInvoiceItemDto
            {
                InvoiceItemID = 2,
                MedicationID = 1,
                MedicationName = "Paracetamol 500mg",
                Quantity = 10,
                LineTotal = 59.90m
            }
        }
    };

    public static PaginatedResponse<InvoiceListDto> InvoicesPaginatedExample => new(
        new List<InvoiceListDto>
        {
            new InvoiceListDto
            {
                InvoiceID = 1,
                PatientName = "John Doe",
                InvoiceTypeName = "Service",
                InvoiceDate = new DateOnly(2024, 12, 15),
                TotalAmount = 225.50m,
                PaymentStatusName = "Pending"
            },
            new InvoiceListDto
            {
                InvoiceID = 2,
                PatientName = "Jane Smith",
                InvoiceTypeName = "Service",
                InvoiceDate = new DateOnly(2024, 12, 16),
                TotalAmount = 75.50m,
                PaymentStatusName = "Paid"
            }
        },
        1, 10, 2
    );
}

