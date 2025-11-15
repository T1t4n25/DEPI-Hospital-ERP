namespace HospitalERP.API.Models.Entities;

public class HospitalInvoiceItem
{
    public int InvoiceItemID { get; set; }
    public int InvoiceID { get; set; }
    public int ServiceID { get; set; }
    public decimal LineTotal { get; set; }

    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
    public Service Service { get; set; } = null!;
}

