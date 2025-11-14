namespace HospitalERP.API.Models.Entities;

public class MedicalRecord
{
    public int RecordID { get; set; }
    public int PatientID { get; set; }
    public int DoctorID { get; set; }
    public int Diagnosesid { get; set; }
    public DateOnly DiagnoseDate { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Employee Doctor { get; set; } = null!;
    public Diagnosis Diagnosis { get; set; } = null!;
}

