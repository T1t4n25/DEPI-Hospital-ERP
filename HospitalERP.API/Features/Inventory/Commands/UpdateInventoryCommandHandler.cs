using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Inventory.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Inventory.Commands;

public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, InventoryDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateInventoryCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InventoryDetailDto> Handle(
        UpdateInventoryCommand request,
        CancellationToken cancellationToken)
    {
        var inventory = await _context.Inventories
            .FirstOrDefaultAsync(i => i.MedicationID == request.Dto.MedicationID, cancellationToken);

        if (inventory == null)
        {
            throw new NotFoundException(nameof(Models.Entities.Inventory), request.Dto.MedicationID);
        }

        _mapper.Map(request.Dto, inventory);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedInventory = await _context.Inventories
            .Include(i => i.Medication)
            .FirstAsync(i => i.MedicationID == inventory.MedicationID, cancellationToken);

        return _mapper.Map<InventoryDetailDto>(updatedInventory);
    }
}

