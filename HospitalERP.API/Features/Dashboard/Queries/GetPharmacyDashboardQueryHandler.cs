using HospitalERP.API.Data;
using HospitalERP.API.Features.Dashboard.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Dashboard.Queries;

public class GetPharmacyDashboardQueryHandler : IRequestHandler<GetPharmacyDashboardQuery, PharmacyDashboardDto>
{
    private readonly HospitalDbContext _context;

    public GetPharmacyDashboardQueryHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<PharmacyDashboardDto> Handle(
        GetPharmacyDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var thirtyDaysFromNow = today.AddDays(30);

        // Total Medications
        var totalMedications = await _context.Medications
            .AsNoTracking()
            .CountAsync(cancellationToken);

        // Get all inventory with medications
        var inventoryData = await _context.Inventories
            .AsNoTracking()
            .Include(i => i.Medication)
            .ToListAsync(cancellationToken);

        // Low Stock Items (Quantity < 10)
        var lowStockItems = inventoryData
            .Where(i => i.Quantity < 10)
            .Select(i => new LowStockItemDto
            {
                MedicationID = i.MedicationID,
                MedicationName = i.Medication.Name,
                Quantity = i.Quantity
            })
            .ToList();

        var lowStockCount = lowStockItems.Count;

        // Calculate expired and expiring soon items
        var expiredItems = new List<ExpiringItemDto>();
        var expiringSoonItems = new List<ExpiringItemDto>();

        foreach (var item in inventoryData)
        {
            var daysUntilExpiry = item.ExpiryDate.DayNumber - today.DayNumber;

            if (daysUntilExpiry < 0)
            {
                // Expired
                expiredItems.Add(new ExpiringItemDto
                {
                    MedicationID = item.MedicationID,
                    MedicationName = item.Medication.Name,
                    ExpiryDate = item.ExpiryDate,
                    DaysUntilExpiry = daysUntilExpiry,
                    Quantity = item.Quantity
                });
            }
            else if (daysUntilExpiry <= 30)
            {
                // Expiring within 30 days (but not expired)
                expiringSoonItems.Add(new ExpiringItemDto
                {
                    MedicationID = item.MedicationID,
                    MedicationName = item.Medication.Name,
                    ExpiryDate = item.ExpiryDate,
                    DaysUntilExpiry = daysUntilExpiry,
                    Quantity = item.Quantity
                });
            }
        }

        // Total Value (Quantity * Cost)
        var totalValue = inventoryData
            .Sum(i => i.Quantity * i.Medication.Cost);

        return new PharmacyDashboardDto
        {
            TotalMedications = totalMedications,
            LowStockItems = lowStockCount,
            ExpiredItems = expiredItems.Count,
            ExpiringSoonItems = expiringSoonItems.Count,
            TotalValue = totalValue,
            LowStockItemsList = lowStockItems,
            ExpiringItemsList = expiredItems.Concat(expiringSoonItems)
                .OrderBy(i => i.DaysUntilExpiry)
                .ToList()
        };
    }
}

