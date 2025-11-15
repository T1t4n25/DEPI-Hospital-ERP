namespace HospitalERP.API.Models.Entities;

public class InvoiceType
{
    public int InvoiceTypeID { get; set; }
    public string InvoiceName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

