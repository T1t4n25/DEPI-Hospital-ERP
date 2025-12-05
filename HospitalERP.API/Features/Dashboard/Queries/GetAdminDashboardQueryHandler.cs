using HospitalERP.API.Data;
using HospitalERP.API.Features.Dashboard.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Dashboard.Queries;

public class GetAdminDashboardQueryHandler : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
{
    private readonly HospitalDbContext _context;

    public GetAdminDashboardQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<AdminDashboardDto> Handle(
        GetAdminDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
        var startOfMonth = new DateOnly(today.Year, today.Month, 1);
        var sixMonthsAgo = startOfMonth.AddMonths(-6);

        // Total Patients (not deleted)
        var totalPatients = await _context.Patients
            .AsNoTracking()
            .CountAsync(p => !p.Deleted, cancellationToken);

        // Today's Appointments
        var todayAppointments = await _context.Appointments
            .AsNoTracking()
            .CountAsync(a => a.AppointmentDateTime.Date == DateTime.Today, cancellationToken);

        // Get payment status ID for "Paid"
        var paidStatus = await _context.PaymentStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.StatusName == "Paid", cancellationToken);
        
        var paidStatusId = paidStatus?.PaymentStatusID ?? 2; // Default to 2 if not found

        // Total Revenue (from paid invoices)
        var totalRevenue = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.PaymentStatusID == paidStatusId)
            .SumAsync(i => i.TotalAmount, cancellationToken);

        // Active Employees
        var activeEmployees = await _context.Employees
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Total Departments
        var totalDepartments = await _context.Departments
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Total Services
        var totalServices = await _context.Services
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Pending Invoices
        var pendingInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(i => i.PaymentStatusID != paidStatusId, cancellationToken); // Not paid

        // Weekly Appointments (last 7 days)
        var weeklyAppointmentsData = await _context.Appointments
            .AsNoTracking()
            .Where(a => a.AppointmentDateTime.Date >= DateTime.Today.AddDays(-7))
            .GroupBy(a => a.AppointmentDateTime.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var weeklyAppointments = weeklyAppointmentsData
            .Select(g => new WeeklyAppointmentStatDto
            {
                Day = g.Date.ToString("ddd"),
                Count = g.Count
            })
            .ToList();

        // Monthly Revenue (last 6 months)
        var monthlyRevenueData = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.InvoiceDate >= sixMonthsAgo && i.PaymentStatusID == paidStatusId)
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new 
            { 
                Year = g.Key.Year, 
                Month = g.Key.Month, 
                Amount = g.Sum(i => i.TotalAmount) 
            })
            .ToListAsync(cancellationToken);

        var monthlyRevenue = monthlyRevenueData
            .Select(g => new MonthlyRevenueDto
            {
                Month = new DateOnly(g.Year, g.Month, 1).ToString("MMM yyyy"),
                Amount = g.Amount
            })
            .OrderBy(r => r.Month)
            .ToList();

        // Department Stats
        var departmentStats = await _context.Employees
            .AsNoTracking()
            .Include(e => e.Department)
            .GroupBy(e => e.Department.DepartmentName)
            .Select(g => new DepartmentStatDto
            {
                DepartmentName = g.Key,
                EmployeeCount = g.Count()
            })
            .ToListAsync(cancellationToken);

        // Recent Activities (last 10 activities from different sources)
        var recentActivities = new List<RecentActivityDto>();

        // Recent appointments
        var recentAppointmentsData = await _context.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .OrderByDescending(a => a.AppointmentDateTime)
            .Take(5)
            .Select(a => new 
            { 
                PatientFirstName = a.Patient.FirstName, 
                PatientLastName = a.Patient.LastName,
                Timestamp = a.AppointmentDateTime
            })
            .ToListAsync(cancellationToken);

        var recentAppointments = recentAppointmentsData
            .Select(a => new RecentActivityDto
            {
                Type = "Appointment",
                Description = $"Appointment scheduled for {a.PatientFirstName} {a.PatientLastName}",
                Timestamp = a.Timestamp
            })
            .ToList();

        recentActivities.AddRange(recentAppointments);

        // Recent invoices
        var recentInvoicesData = await _context.Invoices
            .AsNoTracking()
            .Include(i => i.Patient)
            .OrderByDescending(i => i.InvoiceDate)
            .Take(3)
            .Select(i => new 
            { 
                InvoiceID = i.InvoiceID,
                PatientFirstName = i.Patient.FirstName, 
                PatientLastName = i.Patient.LastName,
                InvoiceDate = i.InvoiceDate
            })
            .ToListAsync(cancellationToken);

        var recentInvoices = recentInvoicesData
            .Select(i => new RecentActivityDto
            {
                Type = "Invoice",
                Description = $"Invoice #{i.InvoiceID} created for {i.PatientFirstName} {i.PatientLastName}",
                Timestamp = i.InvoiceDate.ToDateTime(TimeOnly.MinValue)
            })
            .ToList();

        recentActivities.AddRange(recentInvoices);

        // Recent medical records
        var recentMedicalRecords = await _context.MedicalRecords
            .AsNoTracking()
            .Include(mr => mr.Patient)
            .OrderByDescending(mr => mr.DiagnoseDate)
            .Take(3)
            .Select(mr => new RecentActivityDto
            {
                Type = "Medical Record",
                Description = $"Medical record updated for Patient #{mr.PatientID}",
                Timestamp = mr.DiagnoseDate.ToDateTime(TimeOnly.MinValue)
            })
            .ToListAsync(cancellationToken);

        recentActivities.AddRange(recentMedicalRecords);

        // Order and take top 10
        var activities = recentActivities
            .OrderByDescending(a => a.Timestamp)
            .Take(10)
            .ToList();

        return new AdminDashboardDto
        {
            TotalPatients = totalPatients,
            TodayAppointments = todayAppointments,
            TotalRevenue = totalRevenue,
            ActiveEmployees = activeEmployees,
            TotalDepartments = totalDepartments,
            TotalServices = totalServices,
            PendingInvoices = pendingInvoices,
            WeeklyAppointments = weeklyAppointments,
            MonthlyRevenue = monthlyRevenue,
            DepartmentStats = departmentStats,
            RecentActivities = activities
        };
    }
}

