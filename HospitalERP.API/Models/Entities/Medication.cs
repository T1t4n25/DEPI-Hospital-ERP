namespace HospitalERP.API.Models.Entities;

public class Medication
{
    public int MedicationID { get; set; }
    public string BarCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }

    // Navigation properties
    public Inventory? Inventory { get; set; }
    public ICollection<MedicationInvoiceItem> MedicationInvoiceItems { get; set; } = new List<MedicationInvoiceItem>();
}

