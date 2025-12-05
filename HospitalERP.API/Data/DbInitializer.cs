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
            new Gender { GenderName = "Female" }
        };
        context.Genders.AddRange(genders);
        await context.SaveChangesAsync();

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
        await context.SaveChangesAsync();

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
            new Department { DepartmentName = "Neurology" },
            new Department { DepartmentName = "Oncology" },
            new Department { DepartmentName = "General Surgery" },
            new Department { DepartmentName = "Radiology" },
            new Department { DepartmentName = "Laboratory" },
            new Department { DepartmentName = "Pharmacy" }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        // Get lookup values
        var maleGender = context.Genders.First(g => g.GenderName == "Male");
        var femaleGender = context.Genders.First(g => g.GenderName == "Female");
        var adminRole = context.Roles.First(r => r.RoleName == "Admin");
        var doctorRole = context.Roles.First(r => r.RoleName == "Doctor");
        var nurseRole = context.Roles.First(r => r.RoleName == "Nurse");
        var receptionistRole = context.Roles.First(r => r.RoleName == "Receptionist");
        var pharmacistRole = context.Roles.First(r => r.RoleName == "Pharmacist");
        var accountantRole = context.Roles.First(r => r.RoleName == "Accountant");
        
        var emergencyDept = context.Departments.First(d => d.DepartmentName == "Emergency");
        var cardiologyDept = context.Departments.First(d => d.DepartmentName == "Cardiology");
        var pediatricsDept = context.Departments.First(d => d.DepartmentName == "Pediatrics");
        var orthopedicsDept = context.Departments.First(d => d.DepartmentName == "Orthopedics");
        var neurologyDept = context.Departments.First(d => d.DepartmentName == "Neurology");
        var pharmacyDept = context.Departments.First(d => d.DepartmentName == "Pharmacy");

        // Seed Employees (with various roles)
        var employees = new[]
        {
            // Admins
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
                FirstName = "Emily",
                LastName = "Johnson",
                ContactNumber = "1234567891",
                HireDate = new DateOnly(2018, 3, 15),
                GenderID = femaleGender.GenderID,
                RoleID = adminRole.RoleID,
                DepartmentID = cardiologyDept.DepartmentID
            },
            // Doctors
            new Employee
            {
                FirstName = "Sarah",
                LastName = "Williams",
                ContactNumber = "1234567892",
                HireDate = new DateOnly(2010, 8, 15),
                GenderID = femaleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = cardiologyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Michael",
                LastName = "Brown",
                ContactNumber = "1234567893",
                HireDate = new DateOnly(2018, 3, 1),
                GenderID = maleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "David",
                LastName = "Jones",
                ContactNumber = "1234567894",
                HireDate = new DateOnly(2012, 5, 10),
                GenderID = maleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = pediatricsDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Jennifer",
                LastName = "Garcia",
                ContactNumber = "1234567895",
                HireDate = new DateOnly(2015, 9, 20),
                GenderID = femaleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = orthopedicsDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Robert",
                LastName = "Miller",
                ContactNumber = "1234567896",
                HireDate = new DateOnly(2016, 1, 15),
                GenderID = maleGender.GenderID,
                RoleID = doctorRole.RoleID,
                DepartmentID = neurologyDept.DepartmentID
            },
            // Nurses
            new Employee
            {
                FirstName = "Lisa",
                LastName = "Davis",
                ContactNumber = "1234567897",
                HireDate = new DateOnly(2019, 4, 1),
                GenderID = femaleGender.GenderID,
                RoleID = nurseRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "James",
                LastName = "Wilson",
                ContactNumber = "1234567898",
                HireDate = new DateOnly(2020, 2, 10),
                GenderID = maleGender.GenderID,
                RoleID = nurseRole.RoleID,
                DepartmentID = cardiologyDept.DepartmentID
            },
            // Receptionists
            new Employee
            {
                FirstName = "Patricia",
                LastName = "Moore",
                ContactNumber = "1234567899",
                HireDate = new DateOnly(2021, 6, 1),
                GenderID = femaleGender.GenderID,
                RoleID = receptionistRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "Richard",
                LastName = "Taylor",
                ContactNumber = "1234567900",
                HireDate = new DateOnly(2020, 8, 15),
                GenderID = maleGender.GenderID,
                RoleID = receptionistRole.RoleID,
                DepartmentID = pediatricsDept.DepartmentID
            },
            // Pharmacists
            new Employee
            {
                FirstName = "Jessica",
                LastName = "Anderson",
                ContactNumber = "1234567901",
                HireDate = new DateOnly(2017, 3, 1),
                GenderID = femaleGender.GenderID,
                RoleID = pharmacistRole.RoleID,
                DepartmentID = pharmacyDept.DepartmentID
            },
            new Employee
            {
                FirstName = "William",
                LastName = "Thomas",
                ContactNumber = "1234567902",
                HireDate = new DateOnly(2019, 5, 10),
                GenderID = maleGender.GenderID,
                RoleID = pharmacistRole.RoleID,
                DepartmentID = pharmacyDept.DepartmentID
            },
            // Accountants
            new Employee
            {
                FirstName = "Amanda",
                LastName = "Jackson",
                ContactNumber = "1234567903",
                HireDate = new DateOnly(2018, 1, 1),
                GenderID = femaleGender.GenderID,
                RoleID = accountantRole.RoleID,
                DepartmentID = emergencyDept.DepartmentID
            }
        };
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        // Set department managers
        var johnSmith = context.Employees.First(e => e.FirstName == "John" && e.LastName == "Smith");
        var sarahWilliams = context.Employees.First(e => e.FirstName == "Sarah" && e.LastName == "Williams");
        var davidJones = context.Employees.First(e => e.FirstName == "David" && e.LastName == "Jones");
        var jenniferGarcia = context.Employees.First(e => e.FirstName == "Jennifer" && e.LastName == "Garcia");
        var robertMiller = context.Employees.First(e => e.FirstName == "Robert" && e.LastName == "Miller");
        var jessicaAnderson = context.Employees.First(e => e.FirstName == "Jessica" && e.LastName == "Anderson");

        emergencyDept.ManagerID = johnSmith.EmployeeID;
        cardiologyDept.ManagerID = sarahWilliams.EmployeeID;
        pediatricsDept.ManagerID = davidJones.EmployeeID;
        orthopedicsDept.ManagerID = jenniferGarcia.EmployeeID;
        neurologyDept.ManagerID = robertMiller.EmployeeID;
        pharmacyDept.ManagerID = jessicaAnderson.EmployeeID;
        await context.SaveChangesAsync();

        // Seed Patients (15 patients)
        var bloodTypeA = context.BloodTypes.First(b => b.BloodTypeName == "A+");
        var bloodTypeAP = context.BloodTypes.First(b => b.BloodTypeName == "A-");
        var bloodTypeB = context.BloodTypes.First(b => b.BloodTypeName == "B+");
        var bloodTypeO = context.BloodTypes.First(b => b.BloodTypeName == "O+");
        var bloodTypeAB = context.BloodTypes.First(b => b.BloodTypeName == "AB+");

        var patients = new[]
        {
            new Patient
            {
                FirstName = "Alice",
                LastName = "Williams",
                ContactNumber = "9876543210",
                DateOfBirth = new DateOnly(1990, 5, 12),
                Address = "123 Main St, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Robert",
                LastName = "Davis",
                ContactNumber = "9876543211",
                DateOfBirth = new DateOnly(1985, 9, 25),
                Address = "456 Oak Ave, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            },
            new Patient
            {
                FirstName = "Maria",
                LastName = "Rodriguez",
                ContactNumber = "9876543212",
                DateOfBirth = new DateOnly(1995, 3, 18),
                Address = "789 Pine Rd, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeB.BloodTypeID
            },
            new Patient
            {
                FirstName = "Christopher",
                LastName = "Martinez",
                ContactNumber = "9876543213",
                DateOfBirth = new DateOnly(1988, 7, 8),
                Address = "321 Elm St, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeAB.BloodTypeID
            },
            new Patient
            {
                FirstName = "Elizabeth",
                LastName = "Hernandez",
                ContactNumber = "9876543214",
                DateOfBirth = new DateOnly(1992, 11, 30),
                Address = "654 Maple Dr, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Daniel",
                LastName = "Lopez",
                ContactNumber = "9876543215",
                DateOfBirth = new DateOnly(1980, 2, 14),
                Address = "987 Cedar Ln, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            },
            new Patient
            {
                FirstName = "Jessica",
                LastName = "Gonzalez",
                ContactNumber = "9876543216",
                DateOfBirth = new DateOnly(1998, 6, 22),
                Address = "147 Birch Way, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeB.BloodTypeID
            },
            new Patient
            {
                FirstName = "Matthew",
                LastName = "Wilson",
                ContactNumber = "9876543217",
                DateOfBirth = new DateOnly(1993, 4, 5),
                Address = "258 Spruce Ct, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Ashley",
                LastName = "Anderson",
                ContactNumber = "9876543218",
                DateOfBirth = new DateOnly(1987, 9, 16),
                Address = "369 Willow Ave, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            },
            new Patient
            {
                FirstName = "Joshua",
                LastName = "Thomas",
                ContactNumber = "9876543219",
                DateOfBirth = new DateOnly(1991, 1, 28),
                Address = "741 Ash St, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeAB.BloodTypeID
            },
            new Patient
            {
                FirstName = "Samantha",
                LastName = "Jackson",
                ContactNumber = "9876543220",
                DateOfBirth = new DateOnly(1996, 8, 9),
                Address = "852 Poplar Rd, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Andrew",
                LastName = "White",
                ContactNumber = "9876543221",
                DateOfBirth = new DateOnly(1984, 12, 3),
                Address = "963 Fir Ln, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            },
            new Patient
            {
                FirstName = "Melissa",
                LastName = "Harris",
                ContactNumber = "9876543222",
                DateOfBirth = new DateOnly(1994, 5, 19),
                Address = "159 Hemlock Dr, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeB.BloodTypeID
            },
            new Patient
            {
                FirstName = "Joseph",
                LastName = "Martin",
                ContactNumber = "9876543223",
                DateOfBirth = new DateOnly(1989, 10, 7),
                Address = "357 Cypress Way, City, State 12345",
                GenderID = maleGender.GenderID,
                BloodTypeID = bloodTypeA.BloodTypeID
            },
            new Patient
            {
                FirstName = "Nicole",
                LastName = "Thompson",
                ContactNumber = "9876543224",
                DateOfBirth = new DateOnly(1997, 3, 24),
                Address = "468 Magnolia Ct, City, State 12345",
                GenderID = femaleGender.GenderID,
                BloodTypeID = bloodTypeO.BloodTypeID
            }
        };
        context.Patients.AddRange(patients);
        await context.SaveChangesAsync();

        // Seed Services (more services for different departments)
        var services = new[]
        {
            // Emergency Department Services
            new Service { ServiceName = "General Consultation", Cost = 100.00m, DepartmentID = emergencyDept.DepartmentID },
            new Service { ServiceName = "Emergency Treatment", Cost = 300.00m, DepartmentID = emergencyDept.DepartmentID },
            new Service { ServiceName = "X-Ray", Cost = 150.00m, DepartmentID = emergencyDept.DepartmentID },
            new Service { ServiceName = "CT Scan", Cost = 500.00m, DepartmentID = emergencyDept.DepartmentID },
            new Service { ServiceName = "Blood Test", Cost = 75.00m, DepartmentID = emergencyDept.DepartmentID },
            
            // Cardiology Services
            new Service { ServiceName = "ECG (Electrocardiogram)", Cost = 200.00m, DepartmentID = cardiologyDept.DepartmentID },
            new Service { ServiceName = "Echocardiogram", Cost = 450.00m, DepartmentID = cardiologyDept.DepartmentID },
            new Service { ServiceName = "Cardiology Consultation", Cost = 250.00m, DepartmentID = cardiologyDept.DepartmentID },
            new Service { ServiceName = "Stress Test", Cost = 350.00m, DepartmentID = cardiologyDept.DepartmentID },
            
            // Pediatrics Services
            new Service { ServiceName = "Pediatric Consultation", Cost = 150.00m, DepartmentID = pediatricsDept.DepartmentID },
            new Service { ServiceName = "Child Vaccination", Cost = 100.00m, DepartmentID = pediatricsDept.DepartmentID },
            new Service { ServiceName = "Growth Assessment", Cost = 120.00m, DepartmentID = pediatricsDept.DepartmentID },
            
            // Orthopedics Services
            new Service { ServiceName = "Orthopedic Consultation", Cost = 200.00m, DepartmentID = orthopedicsDept.DepartmentID },
            new Service { ServiceName = "Bone Density Scan", Cost = 300.00m, DepartmentID = orthopedicsDept.DepartmentID },
            new Service { ServiceName = "Physical Therapy Session", Cost = 80.00m, DepartmentID = orthopedicsDept.DepartmentID },
            
            // Neurology Services
            new Service { ServiceName = "Neurology Consultation", Cost = 300.00m, DepartmentID = neurologyDept.DepartmentID },
            new Service { ServiceName = "MRI Scan", Cost = 800.00m, DepartmentID = neurologyDept.DepartmentID },
            new Service { ServiceName = "EEG (Electroencephalogram)", Cost = 400.00m, DepartmentID = neurologyDept.DepartmentID }
        };
        context.Services.AddRange(services);
        await context.SaveChangesAsync();

        // Seed Diagnoses
        var diagnoses = new[]
        {
            new Diagnosis { Diagnoses = "Common Cold" },
            new Diagnosis { Diagnoses = "Hypertension" },
            new Diagnosis { Diagnoses = "Fracture" },
            new Diagnosis { Diagnoses = "Pneumonia" },
            new Diagnosis { Diagnoses = "Diabetes Type 2" },
            new Diagnosis { Diagnoses = "Asthma" },
            new Diagnosis { Diagnoses = "Migraine" },
            new Diagnosis { Diagnoses = "Bronchitis" },
            new Diagnosis { Diagnoses = "Arthritis" },
            new Diagnosis { Diagnoses = "Anemia" },
            new Diagnosis { Diagnoses = "Appendicitis" },
            new Diagnosis { Diagnoses = "Gastritis" },
            new Diagnosis { Diagnoses = "Sinusitis" },
            new Diagnosis { Diagnoses = "Urinary Tract Infection" },
            new Diagnosis { Diagnoses = "Skin Infection" }
        };
        context.Diagnoses.AddRange(diagnoses);
        await context.SaveChangesAsync();

        // Seed Medications (20 medications)
        var medications = new[]
        {
            new Medication { Name = "Paracetamol 500mg", Description = "Pain reliever and fever reducer", BarCode = "1234567890123", Cost = 5.50m },
            new Medication { Name = "Amoxicillin 250mg", Description = "Antibiotic medication for bacterial infections", BarCode = "1234567890124", Cost = 12.00m },
            new Medication { Name = "Aspirin 100mg", Description = "Blood thinner and pain reliever", BarCode = "1234567890125", Cost = 3.25m },
            new Medication { Name = "Ibuprofen 400mg", Description = "Anti-inflammatory and pain reliever", BarCode = "1234567890126", Cost = 7.50m },
            new Medication { Name = "Metformin 500mg", Description = "Diabetes medication", BarCode = "1234567890127", Cost = 15.00m },
            new Medication { Name = "Lisinopril 10mg", Description = "Blood pressure medication", BarCode = "1234567890128", Cost = 18.75m },
            new Medication { Name = "Atorvastatin 20mg", Description = "Cholesterol lowering medication", BarCode = "1234567890129", Cost = 22.50m },
            new Medication { Name = "Albuterol Inhaler", Description = "Asthma bronchodilator", BarCode = "1234567890130", Cost = 35.00m },
            new Medication { Name = "Omeprazole 20mg", Description = "Stomach acid reducer", BarCode = "1234567890131", Cost = 14.25m },
            new Medication { Name = "Amoxicillin 500mg", Description = "Broad-spectrum antibiotic", BarCode = "1234567890132", Cost = 16.50m },
            new Medication { Name = "Ciprofloxacin 500mg", Description = "Antibiotic for infections", BarCode = "1234567890133", Cost = 19.75m },
            new Medication { Name = "Prednisone 10mg", Description = "Corticosteroid anti-inflammatory", BarCode = "1234567890134", Cost = 8.50m },
            new Medication { Name = "Amlodipine 5mg", Description = "Calcium channel blocker for hypertension", BarCode = "1234567890135", Cost = 17.00m },
            new Medication { Name = "Metoprolol 50mg", Description = "Beta blocker for heart conditions", BarCode = "1234567890136", Cost = 20.25m },
            new Medication { Name = "Levothyroxine 50mcg", Description = "Thyroid hormone replacement", BarCode = "1234567890137", Cost = 13.50m },
            new Medication { Name = "Gabapentin 300mg", Description = "Neuropathic pain medication", BarCode = "1234567890138", Cost = 24.00m },
            new Medication { Name = "Tramadol 50mg", Description = "Pain relief medication", BarCode = "1234567890139", Cost = 21.75m },
            new Medication { Name = "Furosemide 40mg", Description = "Diuretic for fluid retention", BarCode = "1234567890140", Cost = 6.75m },
            new Medication { Name = "Warfarin 5mg", Description = "Blood thinner anticoagulant", BarCode = "1234567890141", Cost = 9.25m },
            new Medication { Name = "Insulin Glargine", Description = "Long-acting insulin for diabetes", BarCode = "1234567890142", Cost = 45.00m }
        };
        context.Medications.AddRange(medications);
        await context.SaveChangesAsync();

        // Seed Inventory (for all medications)
        var expiryDates = new[]
        {
            new DateOnly(2026, 12, 31),
            new DateOnly(2025, 6, 30),
            new DateOnly(2027, 1, 31),
            new DateOnly(2026, 8, 15),
            new DateOnly(2027, 3, 20),
            new DateOnly(2025, 11, 30),
            new DateOnly(2026, 5, 10),
            new DateOnly(2027, 9, 25),
            new DateOnly(2026, 2, 28),
            new DateOnly(2025, 12, 31),
            new DateOnly(2026, 7, 15),
            new DateOnly(2027, 4, 30),
            new DateOnly(2026, 10, 5),
            new DateOnly(2025, 9, 20),
            new DateOnly(2027, 6, 15),
            new DateOnly(2026, 1, 31),
            new DateOnly(2025, 8, 10),
            new DateOnly(2027, 11, 30),
            new DateOnly(2026, 4, 20),
            new DateOnly(2027, 2, 15)
        };

        var inventoryItems = medications.Select((m, index) => new Inventory
        {
            MedicationID = m.MedicationID,
            Quantity = new Random().Next(50, 500), // Random quantity between 50-500
            ExpiryDate = expiryDates[index % expiryDates.Length]
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

        // Seed Appointments (20 appointments with various statuses)
        var allPatients = context.Patients.ToList();
        var allDoctors = context.Employees.Where(e => e.RoleID == doctorRole.RoleID).ToList();
        var allServices = context.Services.ToList();
        var appointmentStatuses = new[] { "Scheduled", "Completed", "Cancelled" };
        var random = new Random();

        var appointments = new List<Appointment>();
        for (int i = 0; i < 20; i++)
        {
            var patient = allPatients[random.Next(allPatients.Count)];
            var doctor = allDoctors[random.Next(allDoctors.Count)];
            var service = allServices[random.Next(allServices.Count)];
            var status = appointmentStatuses[random.Next(appointmentStatuses.Length)];
            var appointmentDate = DateTime.Now.AddDays(random.Next(-30, 30)).AddHours(random.Next(8, 18));

            appointments.Add(new Appointment
            {
                PatientID = patient.PatientID,
                DoctorID = doctor.EmployeeID,
                ServiceID = service.ServiceID,
                AppointmentDateTime = appointmentDate,
                Status = status
            });
        }
        context.Appointments.AddRange(appointments);
        await context.SaveChangesAsync();

        // Seed Medical Records (15 medical records)
        var allDiagnoses = context.Diagnoses.ToList();
        var medicalRecords = new List<MedicalRecord>();
        for (int i = 0; i < 15; i++)
        {
            var patient = allPatients[random.Next(allPatients.Count)];
            var doctor = allDoctors[random.Next(allDoctors.Count)];
            var diagnosis = allDiagnoses[random.Next(allDiagnoses.Count)];
            var diagnoseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(-90, 0)));

            medicalRecords.Add(new MedicalRecord
            {
                PatientID = patient.PatientID,
                DoctorID = doctor.EmployeeID,
                Diagnosesid = diagnosis.DiagnosesID,
                DiagnoseDate = diagnoseDate
            });
        }
        context.MedicalRecords.AddRange(medicalRecords);
        await context.SaveChangesAsync();

        // Seed Treatments (for some diagnoses)
        var treatments = new[]
        {
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Common Cold").DiagnosesID, TreatmentDescription = "Rest, hydration, and over-the-counter cold medication" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Hypertension").DiagnosesID, TreatmentDescription = "Lifestyle modifications and antihypertensive medication" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Fracture").DiagnosesID, TreatmentDescription = "Immobilization, pain management, and follow-up X-rays" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Pneumonia").DiagnosesID, TreatmentDescription = "Antibiotic therapy and respiratory support" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Diabetes Type 2").DiagnosesID, TreatmentDescription = "Diet modification, exercise, and metformin" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Asthma").DiagnosesID, TreatmentDescription = "Bronchodilators and inhaled corticosteroids" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Migraine").DiagnosesID, TreatmentDescription = "Pain relievers and preventive medications" },
            new Treatment { DiagnosesID = context.Diagnoses.First(d => d.Diagnoses == "Arthritis").DiagnosesID, TreatmentDescription = "NSAIDs, physical therapy, and joint protection" }
        };
        context.Treatments.AddRange(treatments);
        await context.SaveChangesAsync();

        // Seed Invoices (15 invoices with items)
        var allInvoiceTypes = context.InvoiceTypes.ToList();
        var allPaymentStatuses = context.PaymentStatuses.ToList();
        var invoices = new List<Invoice>();

        for (int i = 0; i < 15; i++)
        {
            var patient = allPatients[random.Next(allPatients.Count)];
            var invoiceType = allInvoiceTypes[random.Next(allInvoiceTypes.Count)];
            var paymentStatus = allPaymentStatuses[random.Next(allPaymentStatuses.Count)];
            var invoiceDate = DateOnly.FromDateTime(DateTime.Now.AddDays(random.Next(-60, 0)));
            var payDate = paymentStatus.StatusName == "Paid" ? invoiceDate.AddDays(random.Next(1, 15)) : (DateOnly?)null;

            var invoice = new Invoice
            {
                PatientID = patient.PatientID,
                InvoiceTypeID = invoiceType.InvoiceTypeID,
                InvoiceDate = invoiceDate,
                PaymentStatusID = paymentStatus.PaymentStatusID,
                PayDate = payDate,
                TotalAmount = 0 // Will be calculated from items
            };
            invoices.Add(invoice);
        }
        context.Invoices.AddRange(invoices);
        await context.SaveChangesAsync();

        // Seed Invoice Items
        var hospitalInvoiceItems = new List<HospitalInvoiceItem>();
        var medicationInvoiceItems = new List<MedicationInvoiceItem>();

        foreach (var invoice in invoices)
        {
            decimal totalAmount = 0;

            // Add service items (if service or mixed type)
            if (invoice.InvoiceTypeID == context.InvoiceTypes.First(it => it.InvoiceName == "Service").InvoiceTypeID ||
                invoice.InvoiceTypeID == context.InvoiceTypes.First(it => it.InvoiceName == "Mixed").InvoiceTypeID)
            {
                var numServices = random.Next(1, 4);
                for (int j = 0; j < numServices; j++)
                {
                    var service = allServices[random.Next(allServices.Count)];
                    var lineTotal = service.Cost;
                    totalAmount += lineTotal;

                    hospitalInvoiceItems.Add(new HospitalInvoiceItem
                    {
                        InvoiceID = invoice.InvoiceID,
                        ServiceID = service.ServiceID,
                        LineTotal = lineTotal
                    });
                }
            }

            // Add medication items (if medication or mixed type)
            if (invoice.InvoiceTypeID == context.InvoiceTypes.First(it => it.InvoiceName == "Medication").InvoiceTypeID ||
                invoice.InvoiceTypeID == context.InvoiceTypes.First(it => it.InvoiceName == "Mixed").InvoiceTypeID)
            {
                var numMedications = random.Next(1, 5);
                for (int j = 0; j < numMedications; j++)
                {
                    var medication = medications[random.Next(medications.Length)];
                    var quantity = random.Next(1, 10);
                    var lineTotal = medication.Cost * quantity;
                    totalAmount += lineTotal;

                    medicationInvoiceItems.Add(new MedicationInvoiceItem
                    {
                        InvoiceID = invoice.InvoiceID,
                        MedicationID = medication.MedicationID,
                        Quantity = quantity,
                        LineTotal = lineTotal
                    });
                }
            }

            // Update invoice total
            invoice.TotalAmount = totalAmount;
        }
        context.HospitalInvoiceItems.AddRange(hospitalInvoiceItems);
        context.MedicationInvoiceItems.AddRange(medicationInvoiceItems);
        await context.SaveChangesAsync();

        // Seed Employee Schedules (for some employees)
        var employeeSchedules = new List<EmployeeSchedule>();
        var shifts = new[]
        {
            (new TimeOnly(8, 0), new TimeOnly(16, 0)), // Morning shift
            (new TimeOnly(16, 0), new TimeOnly(0, 0)), // Afternoon shift
            (new TimeOnly(0, 0), new TimeOnly(8, 0))   // Night shift
        };

        foreach (var employee in context.Employees.Take(10))
        {
            var shift = shifts[random.Next(shifts.Length)];
            employeeSchedules.Add(new EmployeeSchedule
            {
                EmployeeID = employee.EmployeeID,
                ShiftStart = shift.Item1,
                ShiftEnd = shift.Item2
            });
        }
        context.EmployeeSchedules.AddRange(employeeSchedules);
        await context.SaveChangesAsync();
    }
}
