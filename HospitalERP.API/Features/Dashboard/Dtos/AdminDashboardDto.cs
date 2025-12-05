namespace HospitalERP.API.Features.Dashboard.Dtos;

public record AdminDashboardDto
{
    public int TotalPatients { get; init; }
    public int TodayAppointments { get; init; }
    public decimal TotalRevenue { get; init; }
    public int ActiveEmployees { get; init; }
    public int TotalDepartments { get; init; }
    public int TotalServices { get; init; }
    public int PendingInvoices { get; init; }
    public List<WeeklyAppointmentStatDto> WeeklyAppointments { get; init; } = new();
    public List<MonthlyRevenueDto> MonthlyRevenue { get; init; } = new();
    public List<DepartmentStatDto> DepartmentStats { get; init; } = new();
    public List<RecentActivityDto> RecentActivities { get; init; } = new();
}

public record WeeklyAppointmentStatDto
{
    public string Day { get; init; } = string.Empty;
    public int Count { get; init; }
}

public record MonthlyRevenueDto
{
    public string Month { get; init; } = string.Empty;
    public decimal Amount { get; init; }
}

public record DepartmentStatDto
{
    public string DepartmentName { get; init; } = string.Empty;
    public int EmployeeCount { get; init; }
}

public record RecentActivityDto
{
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; }
}

