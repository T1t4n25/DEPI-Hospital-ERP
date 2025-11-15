namespace HospitalERP.API.Models.Entities;

public class PaymentStatus
{
    public int PaymentStatusID { get; set; }
    public string StatusName { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}

