namespace HospitalERP.API.Features.Dashboard.Dtos;

public record HrDashboardDto
{
    public int TotalEmployees { get; init; }
    public int PresentToday { get; init; }
    public int OnLeave { get; init; }
    public int TotalDepartments { get; init; }
    public List<DepartmentEmployeeCountDto> DepartmentEmployeeCounts { get; init; } = new();
    public List<RoleCountDto> RoleCounts { get; init; } = new();
    public List<RecentHireDto> RecentHires { get; init; } = new();
}

public record DepartmentEmployeeCountDto
{
    public string DepartmentName { get; init; } = string.Empty;
    public int EmployeeCount { get; init; }
}

public record RoleCountDto
{
    public string RoleName { get; init; } = string.Empty;
    public int Count { get; init; }
}

public record RecentHireDto
{
    public int EmployeeID { get; init; }
    public string FullName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
    public string DepartmentName { get; init; } = string.Empty;
    public DateOnly HireDate { get; init; }
}

