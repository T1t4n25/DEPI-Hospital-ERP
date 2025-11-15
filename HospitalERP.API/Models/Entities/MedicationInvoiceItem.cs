namespace HospitalERP.API.Models.Entities;

public class MedicationInvoiceItem
{
    public int InvoiceItemID { get; set; }
    public int InvoiceID { get; set; }
    public int MedicationID { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal { get; set; }

    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
    public Medication Medication { get; set; } = null!;
}

