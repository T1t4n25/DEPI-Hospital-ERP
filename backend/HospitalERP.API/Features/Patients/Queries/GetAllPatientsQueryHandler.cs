using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Patients.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Patients.Queries;

public class GetAllPatientsQueryHandler : IRequestHandler<GetAllPatientsQuery, PaginatedResponse<PatientListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllPatientsQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<PatientListDto>> Handle(
        GetAllPatientsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Patients
            .AsNoTracking()
            .Include(p => p.Gender)
            .Include(p => p.BloodType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(p =>
                p.FirstName.ToLower().Contains(searchTerm) ||
                p.LastName.ToLower().Contains(searchTerm) ||
                p.ContactNumber.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var patients = await query
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var patientDtos = _mapper.Map<List<PatientListDto>>(patients);

        return new PaginatedResponse<PatientListDto>(
            patientDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

