namespace HospitalERP.API.Features.Invoices.Dtos;

public record InvoiceListDto
{
    public int InvoiceID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public string InvoiceTypeName { get; init; } = string.Empty;
    public DateOnly InvoiceDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string PaymentStatusName { get; init; } = string.Empty;
}

