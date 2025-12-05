namespace HospitalERP.API.Features.Dashboard.Dtos;

public record PharmacyDashboardDto
{
    public int TotalMedications { get; init; }
    public int LowStockItems { get; init; }
    public int ExpiredItems { get; init; }
    public int ExpiringSoonItems { get; init; }
    public decimal TotalValue { get; init; }
    public List<LowStockItemDto> LowStockItemsList { get; init; } = new();
    public List<ExpiringItemDto> ExpiringItemsList { get; init; } = new();
}

public record LowStockItemDto
{
    public int MedicationID { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public int Quantity { get; init; }
}

public record ExpiringItemDto
{
    public int MedicationID { get; init; }
    public string MedicationName { get; init; } = string.Empty;
    public DateOnly ExpiryDate { get; init; }
    public int DaysUntilExpiry { get; init; }
    public int Quantity { get; init; }
}

