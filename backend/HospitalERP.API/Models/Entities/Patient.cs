namespace HospitalERP.API.Models.Entities;

public class Patient
{
    public int PatientID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public int GenderID { get; set; }
    public string Address { get; set; } = string.Empty;
    public int BloodTypeID { get; set; }
    public string ContactNumber { get; set; } = string.Empty;

    // Navigation properties
    public Gender Gender { get; set; } = null!;
    public BloodType BloodType { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

