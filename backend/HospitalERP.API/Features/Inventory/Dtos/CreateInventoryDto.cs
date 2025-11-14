namespace HospitalERP.API.Features.Inventory.Dtos;

public record CreateInventoryDto
{
    public int MedicationID { get; init; }
    public int Quantity { get; init; }
    public DateOnly ExpiryDate { get; init; }
}

