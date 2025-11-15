namespace HospitalERP.API.Features.Invoices.Dtos;

public record InvoiceDetailDto
{
    public int InvoiceID { get; init; }
    public int PatientID { get; init; }
    public string PatientName { get; init; } = string.Empty;
    public int InvoiceTypeID { get; init; }
    public string InvoiceTypeName { get; init; } = string.Empty;
    public DateOnly InvoiceDate { get; init; }
    public decimal TotalAmount { get; init; }
    public int PaymentStatusID { get; init; }
    public string PaymentStatusName { get; init; } = string.Empty;
    public DateOnly? PayDate { get; init; }
    public List<HospitalInvoiceItemDto> HospitalItems { get; init; } = new();
    public List<MedicationInvoiceItemDto> MedicationItems { get; init; } = new();
}

public record HospitalInvoiceItemDto
{
    public int InvoiceItemID { get; init; }
    public int ServiceID { get; init; }
    public string ServiceName { get; init; } = string.Empty;
    public decimal LineTotal { get; init; }
}

public record MedicationInvoiceItemDto
{
    public int InvoiceItemID { get; init; }
    public int MedicationID { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public decimal LineTotal { get; init; }
}

