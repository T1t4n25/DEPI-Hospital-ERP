namespace HospitalERP.API.Features.Dashboard.Dtos;

public record AccountantDashboardDto
{
    public decimal TotalRevenue { get; init; }
    public decimal PendingPayments { get; init; }
    public decimal PaidAmount { get; init; }
    public int TotalInvoices { get; init; }
    public int PaidInvoices { get; init; }
    public int PendingInvoices { get; init; }
    public List<MonthlyRevenueDto> MonthlyRevenue { get; init; } = new();
    public List<InvoiceStatusDto> InvoiceStatusBreakdown { get; init; } = new();
    public List<RecentInvoiceDto> RecentInvoices { get; init; } = new();
}

public record InvoiceStatusDto
{
    public string StatusName { get; init; } = string.Empty;
    public int Count { get; init; }
    public decimal TotalAmount { get; init; }
}

public record RecentInvoiceDto
{
    public int InvoiceID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public string PaymentStatusName { get; init; } = string.Empty;
    public DateOnly InvoiceDate { get; init; }
}

