namespace HospitalERP.API.Models.Entities;

public class Role
{
    public int RoleID { get; set; }
    public string RoleName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

