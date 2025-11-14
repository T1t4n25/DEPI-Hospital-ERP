namespace HospitalERP.API.Models.Entities;

public class BloodType
{
    public int BloodTypeID { get; set; }
    public string BloodTypeName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}

