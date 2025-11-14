namespace HospitalERP.API.Models.Entities;

public class Department
{
    public int DepartmentID { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int? ManagerID { get; set; }

    // Navigation properties
    public Employee? Manager { get; set; }
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
}

