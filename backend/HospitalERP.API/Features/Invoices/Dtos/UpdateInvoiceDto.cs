namespace HospitalERP.API.Features.Invoices.Dtos;

public record UpdateInvoiceDto
{
    public int InvoiceID { get; init; }
    public int PaymentStatusID { get; init; }
    public DateOnly? PayDate { get; init; }
}

