namespace HospitalERP.API.Features.Inventory.Dtos;

public record InventoryDetailDto
{
    public int MedicationID { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public string BarCode { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Cost { get; init; }
    public int Quantity { get; init; }
    public DateOnly ExpiryDate { get; init; }
}

