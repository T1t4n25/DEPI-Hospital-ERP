using HospitalERP.API.Data;
using HospitalERP.API.Features.Dashboard.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Dashboard.Queries;

public class GetHrDashboardQueryHandler : IRequestHandler<GetHrDashboardQuery, HrDashboardDto>
{
    private readonly HospitalDbContext _context;

    public GetHrDashboardQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<HrDashboardDto> Handle(
        GetHrDashboardQuery request,
        CancellationToken cancellationToken)
    {
        // Total Employees
        var totalEmployees = await _context.Employees
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Total Departments
        var totalDepartments = await _context.Departments
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Present Today and On Leave (placeholder - would need attendance system)
        // For now, we'll use a simple calculation
        var presentToday = (int)(totalEmployees * 0.9); // 90% present
        var onLeave = totalEmployees - presentToday;

        // Department Employee Counts
        var departmentEmployeeCounts = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .GroupBy(e => e.Department.DepartmentName)
            .Select(g => new DepartmentEmployeeCountDto
            {
                DepartmentName = g.Key ?? "Unassigned",
                EmployeeCount = g.Count()
            })
            .OrderByDescending(d => d.EmployeeCount)
            .ToListAsync(cancellationToken);

        // Role Counts
        var roleCounts = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Role)
            .GroupBy(e => e.Role.RoleName)
            .Select(g => new RoleCountDto
            {
                RoleName = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(r => r.Count)
            .ToListAsync(cancellationToken);

        // Recent Hires (last 30 days)
        var thirtyDaysAgo = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
        var recentHires = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Role)
            .Include(e => e.Department)
            .Where(e => e.HireDate >= thirtyDaysAgo)
            .OrderByDescending(e => e.HireDate)
            .Take(10)
            .Select(e => new RecentHireDto
            {
                EmployeeID = e.EmployeeID,
                FullName = $"{e.FirstName} {e.LastName}",
                RoleName = e.Role.RoleName,
                DepartmentName = e.Department.DepartmentName,
                HireDate = e.HireDate
            })
            .ToListAsync(cancellationToken);

        return new HrDashboardDto
        {
            TotalEmployees = totalEmployees,
            PresentToday = presentToday,
            OnLeave = onLeave,
            TotalDepartments = totalDepartments,
            DepartmentEmployeeCounts = departmentEmployeeCounts,
            RoleCounts = roleCounts,
            RecentHires = recentHires
        };
    }
}

