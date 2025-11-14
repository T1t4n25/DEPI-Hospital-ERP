using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Services.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Services.Queries;

public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, PaginatedResponse<ServiceListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllServicesQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<ServiceListDto>> Handle(
        GetAllServicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Services
            .AsNoTracking()
            .Include(s => s.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(s => s.ServiceName.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var services = await query
            .OrderBy(s => s.ServiceName)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var serviceDtos = _mapper.Map<List<ServiceListDto>>(services);

        return new PaginatedResponse<ServiceListDto>(
            serviceDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

