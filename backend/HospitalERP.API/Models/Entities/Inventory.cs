namespace HospitalERP.API.Models.Entities;

public class Inventory
{
    public int MedicationID { get; set; }
    public int Quantity { get; set; }
    public DateOnly ExpiryDate { get; set; }

    // Navigation properties
    public Medication Medication { get; set; } = null!;
}

