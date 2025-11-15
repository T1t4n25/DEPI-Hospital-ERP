using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Inventory.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Inventory.Commands;

public class CreateInventoryCommandHandler : IRequestHandler<CreateInventoryCommand, InventoryDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreateInventoryCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InventoryDetailDto> Handle(
        CreateInventoryCommand request,
        CancellationToken cancellationToken)
    {
        var medicationExists = await _context.Medications
            .AnyAsync(m => m.MedicationID == request.Dto.MedicationID, cancellationToken);
        if (!medicationExists)
        {
            throw new NotFoundException(nameof(Models.Entities.Medication), request.Dto.MedicationID);
        }

        var inventory = _mapper.Map<Models.Entities.Inventory>(request.Dto);
        _context.Inventories.Add(inventory);
        await _context.SaveChangesAsync(cancellationToken);

        var createdInventory = await _context.Inventories
            .Include(i => i.Medication)
            .FirstAsync(i => i.MedicationID == inventory.MedicationID, cancellationToken);

        return _mapper.Map<InventoryDetailDto>(createdInventory);
    }
}

