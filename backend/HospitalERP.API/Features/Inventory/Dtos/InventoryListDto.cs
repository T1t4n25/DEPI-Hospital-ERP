namespace HospitalERP.API.Features.Inventory.Dtos;

public record InventoryListDto
{
    public int MedicationID { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public string BarCode { get; init; } = string.Empty;
    public int Quantity { get; init; }
    public DateOnly ExpiryDate { get; init; }
}

