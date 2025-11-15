namespace HospitalERP.API.Models.Entities;

public class Invoice
{
    public int InvoiceID { get; set; }
    public int PatientID { get; set; }
    public int InvoiceTypeID { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public int PaymentStatusID { get; set; }
    public DateOnly? PayDate { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public InvoiceType InvoiceType { get; set; } = null!;
    public PaymentStatus PaymentStatus { get; set; } = null!;
    public ICollection<HospitalInvoiceItem> HospitalInvoiceItems { get; set; } = new List<HospitalInvoiceItem>();
    public ICollection<MedicationInvoiceItem> MedicationInvoiceItems { get; set; } = new List<MedicationInvoiceItem>();
}

