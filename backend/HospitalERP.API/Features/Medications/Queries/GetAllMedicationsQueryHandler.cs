using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Medications.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Medications.Queries;

public class GetAllMedicationsQueryHandler : IRequestHandler<GetAllMedicationsQuery, PaginatedResponse<MedicationListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllMedicationsQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<MedicationListDto>> Handle(
        GetAllMedicationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Medications
            .AsNoTracking()
            .Include(m => m.Inventory)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(m =>
                m.Name.ToLower().Contains(searchTerm) ||
                m.BarCode.Contains(searchTerm) ||
                m.Description.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var medications = await query
            .OrderBy(m => m.Name)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var medicationDtos = _mapper.Map<List<MedicationListDto>>(medications);

        return new PaginatedResponse<MedicationListDto>(
            medicationDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

