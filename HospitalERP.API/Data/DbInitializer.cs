using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(HospitalDbContext context)
    {
        // Ensure database is created
        await context.Database.EnsureCreatedAsync();

        // Seed only if tables are empty
        if (context.Genders.Any() || context.Roles.Any())
        {
            return; // Database already seeded
        }

        // Seed Genders
        var genders = new[]
        {
            new Gender { GenderName = "Male" },
            new Gender { GenderName = "Female" },
            new Gender { GenderName = "Other" }
        };
        context.Genders.AddRange(genders);

        // Seed BloodTypes
        var bloodTypes = new[]
        {
            new BloodType { BloodTypeName = "A+" },
            new BloodType { BloodTypeName = "A-" },
            new BloodType { BloodTypeName = "B+" },
            new BloodType { BloodTypeName = "B-" },
            new BloodType { BloodTypeName = "AB+" },
            new BloodType { BloodTypeName = "AB-" },
            new BloodType { BloodTypeName = "O+" },
            new BloodType { BloodTypeName = "O-" }
        };
        context.BloodTypes.AddRange(bloodTypes);

        // Seed Roles
        var roles = new[]
        {
            new Role { RoleName = "Admin" },
            new Role { RoleName = "Doctor" },
            new Role { RoleName = "Nurse" },
            new Role { RoleName = "Receptionist" },
            new Role { RoleName = "Pharmacist" },
            new Role { RoleName = "Accountant" }
        };
        context.Roles.AddRange(roles);

        await context.SaveChangesAsync();

        // Seed Departments
        var departments = new[]
        {
            new Department { DepartmentName = "Emergency" },
            new Department { DepartmentName = "Cardiology" },
            new Department { DepartmentName = "Pediatrics" },
            new Department { DepartmentName = "Orthopedics" },
            new Department { DepartmentName = "Neurology" }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // Seed Employees (Doctors and Admin)
        var adminRole = context.Roles.First(r => r.RoleName == "Admin");
        var doctorRole = context.Roles.First(r => r.RoleName == "Doctor");
        var emergencyDept = context.Departments.First(d => d.DepartmentName == "Emergency");
        var cardiologyDept = context.Departments.First(d => d.DepartmentName == "Cardiology");
        var maleGender = context.Genders.First(g => g.GenderName == "Male");
        var femaleGender = context.Genders.First(g => g.GenderName == "Female");

        var employees = new[]
        {
            new Employee
            {
                FirstName = "John",
                LastName = "Smith",
                ContactNumber = "1234567890",
                HireDate = new DateOnly(2015, 6, 1),
                GenderID = maleGender.GenderID,
                RoleID = adminRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Sarah",
                LastName = "Johnson",
                ContactNumber = "1234567891",
                HireDate = new DateOnly(2010, 8, 15),
                GenderID = femaleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = cardiologyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Michael",
                LastName = "Brown",
                ContactNumber = "1234567892",
                HireDate = new DateOnly(2018, 3, 1),
                GenderID = maleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            }
        };
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        // Seed Patients
        var bloodTypeA = context.BloodTypes.First(b => b.BloodTypeName == "A+");
        var bloodTypeO = context.BloodTypes.First(b => b.BloodTypeName == "O+");

        var patients = new[]
        {
            new Patient
            {
                FirstName = "Alice",
                LastName = "Williams",
                ContactNumber = "9876543210",
                DateOfBirth = new DateOnly(1990, 5, 12),
                Address = "123 Main St, City, State",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Robert",
                LastName = "Davis",
                ContactNumber = "9876543211",
                DateOfBirth = new DateOnly(1985, 9, 25),
                Address = "456 Oak Ave, City, State",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            }
        };
        context.Patients.AddRange(patients);
        await context.SaveChangesAsync();

        // Seed Services
        var services = new[]
        {
            new Service { ServiceName = "General Consultation", Cost = 100.00m, DepartmentID = emergencyDept.DepartmentID },
            new Service { ServiceName = "Cardiology Consultation", Cost = 200.00m, DepartmentID = cardiologyDept.DepartmentID },
            new Service { ServiceName = "X-Ray", Cost = 150.00m, DepartmentID = emergencyDept.DepartmentID }
        };
        context.Services.AddRange(services);
        await context.SaveChangesAsync();

        // Seed Diagnoses
        var diagnoses = new[]
        {
            new Diagnosis { Diagnoses = "Common Cold" },
            new Diagnosis { Diagnoses = "Hypertension" },
            new Diagnosis { Diagnoses = "Fracture" }
        };
        context.Diagnoses.AddRange(diagnoses);
        await context.SaveChangesAsync();

        // Seed Medications
        var medications = new[]
        {
            new Medication { Name = "Paracetamol", Description = "Pain reliever and fever reducer", BarCode = "1234567890123", Cost = 5.50m },
            new Medication { Name = "Amoxicillin", Description = "Antibiotic medication", BarCode = "1234567890124", Cost = 12.00m },
            new Medication { Name = "Aspirin", Description = "Blood thinner and pain reliever", BarCode = "1234567890125", Cost = 3.25m }
        };
        context.Medications.AddRange(medications);
        await context.SaveChangesAsync();

        // Seed Inventory
        var expiryDates = new[]
        {
            new DateOnly(2026, 12, 31),
            new DateOnly(2025, 6, 30),
            new DateOnly(2027, 1, 31)
        };

        var inventoryItems = medications.Select((m, index) => new Inventory
        {
            MedicationID = m.MedicationID,
            Quantity = 100,
            ExpiryDate = expiryDates[index]
        }).ToArray();
        context.Inventories.AddRange(inventoryItems);
        await context.SaveChangesAsync();

        // Seed InvoiceTypes
        var invoiceTypes = new[]
        {
            new InvoiceType { InvoiceName = "Service" },
            new InvoiceType { InvoiceName = "Medication" },
            new InvoiceType { InvoiceName = "Mixed" }
        };
        context.InvoiceTypes.AddRange(invoiceTypes);
        await context.SaveChangesAsync();

        // Seed PaymentStatuses
        var paymentStatuses = new[]
        {
            new PaymentStatus { StatusName = "Pending" },
            new PaymentStatus { StatusName = "Paid" },
            new PaymentStatus { StatusName = "Cancelled" }
        };
        context.PaymentStatuses.AddRange(paymentStatuses);
        await context.SaveChangesAsync();
    }
}

