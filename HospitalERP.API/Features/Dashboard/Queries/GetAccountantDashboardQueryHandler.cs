using HospitalERP.API.Data;
using HospitalERP.API.Features.Dashboard.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Dashboard.Queries;

public class GetAccountantDashboardQueryHandler : IRequestHandler<GetAccountantDashboardQuery, AccountantDashboardDto>
{
    private readonly HospitalDbContext _context;

    public GetAccountantDashboardQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<AccountantDashboardDto> Handle(
        GetAccountantDashboardQuery request,
        CancellationToken cancellationToken)
    {
        // Get payment status IDs (they might vary, so we query by name)
        var paidStatus = await _context.PaymentStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.StatusName == "Paid", cancellationToken);
        
        var paidStatusId = paidStatus?.PaymentStatusID ?? 2; // Default to 2 if not found

        // Total Revenue (from paid invoices)
        var totalRevenue = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.PaymentStatusID == paidStatusId)
            .SumAsync(i => i.TotalAmount, cancellationToken);

        // Pending Payments (from non-paid, non-cancelled invoices)
        var pendingPayments = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.PaymentStatusID != paidStatusId)
            .SumAsync(i => i.TotalAmount, cancellationToken);

        // Paid Amount
        var paidAmount = totalRevenue;

        // Total Invoices
        var totalInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Paid Invoices
        var paidInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(i => i.PaymentStatusID == paidStatusId, cancellationToken);

        // Pending Invoices
        var pendingInvoices = await _context.Invoices
            .AsNoTracking()
            .CountAsync(i => i.PaymentStatusID != paidStatusId, cancellationToken);

        // Monthly Revenue (last 6 months from paid invoices)
        var sixMonthsAgo = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-6);
        var monthlyRevenue = await _context.Invoices
            .AsNoTracking()
            .Where(i => i.InvoiceDate >= sixMonthsAgo && i.PaymentStatusID == paidStatusId)
            .GroupBy(i => new { i.InvoiceDate.Year, i.InvoiceDate.Month })
            .Select(g => new MonthlyRevenueDto
            {
                Month = new DateOnly(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                Amount = g.Sum(i => i.TotalAmount)
            })
            .OrderBy(r => r.Month)
            .ToListAsync(cancellationToken);

        // Invoice Status Breakdown
        var invoiceStatusBreakdown = await _context.Invoices
            .AsNoTracking()
            .Include(i => i.PaymentStatus)
            .GroupBy(i => new { i.PaymentStatus.PaymentStatusID, i.PaymentStatus.StatusName })
            .Select(g => new InvoiceStatusDto
            {
                StatusName = g.Key.StatusName,
                Count = g.Count(),
                TotalAmount = g.Sum(i => i.TotalAmount)
            })
            .ToListAsync(cancellationToken);

        // Recent Invoices (last 10)
        var recentInvoices = await _context.Invoices
            .AsNoTracking()
            .Include(i => i.Patient)
            .Include(i => i.PaymentStatus)
            .OrderByDescending(i => i.InvoiceDate)
            .Take(10)
            .Select(i => new RecentInvoiceDto
            {
                InvoiceID = i.InvoiceID,
                PatientName = $"{i.Patient.FirstName} {i.Patient.LastName}",
                TotalAmount = i.TotalAmount,
                PaymentStatusName = i.PaymentStatus.StatusName,
                InvoiceDate = i.InvoiceDate
            })
            .ToListAsync(cancellationToken);

        return new AccountantDashboardDto
        {
            TotalRevenue = totalRevenue,
            PendingPayments = pendingPayments,
            PaidAmount = paidAmount,
            TotalInvoices = totalInvoices,
            PaidInvoices = paidInvoices,
            PendingInvoices = pendingInvoices,
            MonthlyRevenue = monthlyRevenue,
            InvoiceStatusBreakdown = invoiceStatusBreakdown,
            RecentInvoices = recentInvoices
        };
    }
}

