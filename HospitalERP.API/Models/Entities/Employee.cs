namespace HospitalERP.API.Models.Entities;

public class Employee
{
    public int EmployeeID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int GenderID { get; set; }
    public int RoleID { get; set; }
    public int DepartmentID { get; set; }
    public string ContactNumber { get; set; } = string.Empty;
    public DateOnly HireDate { get; set; }

    // Navigation properties
    public Gender Gender { get; set; } = null!;
    public Role Role { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public ICollection<Department> ManagedDepartments { get; set; } = new List<Department>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    public ICollection<EmployeeSchedule> Schedules { get; set; } = new List<EmployeeSchedule>();
}

