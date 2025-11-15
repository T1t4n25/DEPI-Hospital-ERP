namespace HospitalERP.API.Models.Entities;

public class Treatment
{
    public int TreatmentID { get; set; }
    public int DiagnosesID { get; set; }
    public string TreatmentDescription { get; set; } = string.Empty;

    // Navigation properties
    public Diagnosis Diagnosis { get; set; } = null!;
}

