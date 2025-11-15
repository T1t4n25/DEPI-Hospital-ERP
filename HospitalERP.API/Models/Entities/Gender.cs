namespace HospitalERP.API.Models.Entities;

public class Gender
{
    public int GenderID { get; set; }
    public string GenderName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

