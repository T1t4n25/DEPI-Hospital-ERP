using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Inventory.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Inventory.Queries;

public class GetAllInventoryQueryHandler : IRequestHandler<GetAllInventoryQuery, PaginatedResponse<InventoryListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllInventoryQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<InventoryListDto>> Handle(
        GetAllInventoryQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Inventories
            .AsNoTracking()
            .Include(i => i.Medication)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(i =>
                i.Medication.Name.ToLower().Contains(searchTerm) ||
                i.Medication.BarCode.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var inventories = await query
            .OrderBy(i => i.Medication.Name)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var inventoryDtos = _mapper.Map<List<InventoryListDto>>(inventories);

        return new PaginatedResponse<InventoryListDto>(
            inventoryDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

