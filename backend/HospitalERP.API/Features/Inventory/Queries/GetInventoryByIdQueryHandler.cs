using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Inventory.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Inventory.Queries;

public class GetInventoryByIdQueryHandler : IRequestHandler<GetInventoryByIdQuery, InventoryDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetInventoryByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InventoryDetailDto> Handle(
        GetInventoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var inventory = await _context.Inventories
            .AsNoTracking()
            .Include(i => i.Medication)
            .FirstOrDefaultAsync(i => i.MedicationID == request.MedicationID, cancellationToken);

        if (inventory == null)
        {
            throw new NotFoundException(nameof(Models.Entities.Inventory), request.MedicationID);
        }

        return _mapper.Map<InventoryDetailDto>(inventory);
    }
}

