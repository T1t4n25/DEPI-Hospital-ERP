using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Departments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Departments.Queries;

public class GetAllDepartmentsQueryHandler : IRequestHandler<GetAllDepartmentsQuery, PaginatedResponse<DepartmentListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllDepartmentsQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<DepartmentListDto>> Handle(
        GetAllDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Departments
            .AsNoTracking()
            .Include(d => d.Manager)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(d => d.DepartmentName.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var departments = await query
            .OrderBy(d => d.DepartmentName)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var departmentDtos = _mapper.Map<List<DepartmentListDto>>(departments);

        return new PaginatedResponse<DepartmentListDto>(
            departmentDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

