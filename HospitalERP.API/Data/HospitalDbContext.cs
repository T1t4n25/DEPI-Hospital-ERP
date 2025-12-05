using Microsoft.EntityFrameworkCore;
using HospitalERP.API.Models.Entities;

namespace HospitalERP.API.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options)
        : base(options)
    {
    }

    // Lookup Tables
    public DbSet<Gender> Genders { get; set; }
    public DbSet<BloodType> BloodTypes { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<InvoiceType> InvoiceTypes { get; set; }
    public DbSet<PaymentStatus> PaymentStatuses { get; set; }

    // Core Entities
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }

    // Medical Records
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<MedicalRecord> MedicalRecords { get; set; }

    // Appointments & Services
    public DbSet<Service> Services { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    // Medications & Inventory
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    // Invoices & Billing
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<HospitalInvoiceItem> HospitalInvoiceItems { get; set; }
    public DbSet<MedicationInvoiceItem> MedicationInvoiceItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Patient
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientID);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.Deleted).IsRequired().HasDefaultValue(false);

            entity.HasOne(e => e.Gender)
                .WithMany(g => g.Patients)
                .HasForeignKey(e => e.GenderID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.BloodType)
                .WithMany(bt => bt.Patients)
                .HasForeignKey(e => e.BloodTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ContactNumber);
            entity.HasIndex(e => new { e.FirstName, e.LastName });
        });

        // Configure Employee
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeID);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);

            entity.HasOne(e => e.Gender)
                .WithMany(g => g.Employees)
                .HasForeignKey(e => e.GenderID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ContactNumber);
        });

        // Configure Department
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentID);
            entity.Property(e => e.DepartmentName).IsRequired().HasMaxLength(100);

            entity.HasOne(e => e.Manager)
                .WithMany(emp => emp.ManagedDepartments)
                .HasForeignKey(e => e.ManagerID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.DepartmentName).IsUnique();
        });

        // Configure Appointment
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentID);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(e => e.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Doctor)
                .WithMany(emp => emp.Appointments)
                .HasForeignKey(e => e.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(e => e.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.AppointmentDateTime);
            entity.HasIndex(e => e.Status);
        });

        // Configure MedicalRecord
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.RecordID);

            entity.HasOne(e => e.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(e => e.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Doctor)
                .WithMany(emp => emp.MedicalRecords)
                .HasForeignKey(e => e.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Diagnosis)
                .WithMany(d => d.MedicalRecords)
                .HasForeignKey(e => e.Diagnosesid)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.DiagnoseDate);
        });

        // Configure Service
        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceID);
            entity.Property(e => e.ServiceName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Cost).HasPrecision(18, 2);

            entity.HasOne(e => e.Department)
                .WithMany(d => d.Services)
                .HasForeignKey(e => e.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.ServiceName);
        });

        // Configure Medication
        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.MedicationID);
            entity.Property(e => e.BarCode).HasMaxLength(13);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Cost).HasPrecision(18, 2);

            entity.HasIndex(e => e.BarCode).IsUnique();
            entity.HasIndex(e => e.Name);
        });

        // Configure Inventory
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.MedicationID);

            entity.HasOne(e => e.Medication)
                .WithOne(m => m.Inventory)
                .HasForeignKey<Inventory>(e => e.MedicationID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ExpiryDate);
        });

        // Configure Invoice
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.InvoiceID);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(e => e.Patient)
                .WithMany(p => p.Invoices)
                .HasForeignKey(e => e.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.InvoiceType)
                .WithMany(it => it.Invoices)
                .HasForeignKey(e => e.InvoiceTypeID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PaymentStatus)
                .WithMany(ps => ps.Invoices)
                .HasForeignKey(e => e.PaymentStatusID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.InvoiceDate);
            entity.HasIndex(e => e.PaymentStatusID);
        });

        // Configure HospitalInvoiceItem
        modelBuilder.Entity<HospitalInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemID);
            entity.Property(e => e.LineTotal).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.HospitalInvoiceItems)
                .HasForeignKey(e => e.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Service)
                .WithMany(s => s.HospitalInvoiceItems)
                .HasForeignKey(e => e.ServiceID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure MedicationInvoiceItem
        modelBuilder.Entity<MedicationInvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemID);
            entity.Property(e => e.LineTotal).HasPrecision(18, 2);

            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.MedicationInvoiceItems)
                .HasForeignKey(e => e.InvoiceID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Medication)
                .WithMany(m => m.MedicationInvoiceItems)
                .HasForeignKey(e => e.MedicationID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure EmployeeSchedule
        modelBuilder.Entity<EmployeeSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleID);

            entity.HasOne(e => e.Employee)
                .WithMany(emp => emp.Schedules)
                .HasForeignKey(e => e.EmployeeID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Treatment
        modelBuilder.Entity<Treatment>(entity =>
        {
            entity.HasKey(e => e.TreatmentID);
            entity.Property(e => e.TreatmentDescription)
                .IsRequired()
                .HasMaxLength(1000);

            entity.HasOne(e => e.Diagnosis)
                .WithMany(d => d.Treatments)
                .HasForeignKey(e => e.DiagnosesID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Lookup Tables
        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderID);
            entity.Property(e => e.GenderName).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.GenderName).IsUnique();
        });

        modelBuilder.Entity<BloodType>(entity =>
        {
            entity.HasKey(e => e.BloodTypeID);
            entity.Property(e => e.BloodTypeName).IsRequired().HasMaxLength(10);
            entity.HasIndex(e => e.BloodTypeName).IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleID);
            entity.Property(e => e.RoleName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.RoleName).IsUnique();
        });

        modelBuilder.Entity<InvoiceType>(entity =>
        {
            entity.HasKey(e => e.InvoiceTypeID);
            entity.Property(e => e.InvoiceName).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.InvoiceName).IsUnique();
        });

        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.HasKey(e => e.PaymentStatusID);
            entity.Property(e => e.StatusName).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.StatusName).IsUnique();
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosesID);
            entity.Property(e => e.Diagnoses).IsRequired().HasMaxLength(500);
            entity.HasIndex(e => e.Diagnoses);
        });
    }
}

