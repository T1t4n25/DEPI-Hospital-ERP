namespace HospitalERP.API.Models.Entities;

public class Diagnosis
{
    public int DiagnosesID { get; set; }
    public string Diagnoses { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<Treatment> Treatments { get; set; } = new List<Treatment>();
}

