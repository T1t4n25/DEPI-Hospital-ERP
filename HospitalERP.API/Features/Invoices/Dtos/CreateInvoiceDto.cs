namespace HospitalERP.API.Features.Invoices.Dtos;

public record CreateInvoiceDto
{
    public int PatientID { get; init; }
    public int InvoiceTypeID { get; init; }
    public DateOnly InvoiceDate { get; init; }
    public int PaymentStatusID { get; init; }
    public List<CreateHospitalInvoiceItemDto> HospitalItems { get; init; } = new();
    public List<CreateMedicationInvoiceItemDto> MedicationItems { get; init; } = new();
}

public record CreateHospitalInvoiceItemDto
{
    public int ServiceID { get; init; }
    public decimal LineTotal { get; init; }
}

public record CreateMedicationInvoiceItemDto
{
    public int MedicationID { get; init; }
    public int Quantity { get; init; }
    public decimal LineTotal { get; init; }
}

