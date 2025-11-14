using AutoMapper;
using HospitalERP.API.Common.Pagination;
using HospitalERP.API.Data;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.MedicalRecords.Queries;

public class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery, PaginatedResponse<MedicalRecordListDto>>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetAllMedicalRecordsQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<MedicalRecordListDto>> Handle(
        GetAllMedicalRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.MedicalRecords
            .AsNoTracking()
            .Include(mr => mr.Patient)
            .Include(mr => mr.Doctor)
            .Include(mr => mr.Diagnosis)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.QueryParams.SearchTerm))
        {
            var searchTerm = request.QueryParams.SearchTerm.ToLower();
            query = query.Where(mr =>
                mr.Patient.FirstName.ToLower().Contains(searchTerm) ||
                mr.Patient.LastName.ToLower().Contains(searchTerm) ||
                mr.Diagnosis.Diagnoses.ToLower().Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var medicalRecords = await query
            .OrderByDescending(mr => mr.DiagnoseDate)
            .Skip((request.QueryParams.PageNumber - 1) * request.QueryParams.PageSize)
            .Take(request.QueryParams.PageSize)
            .ToListAsync(cancellationToken);

        var medicalRecordDtos = _mapper.Map<List<MedicalRecordListDto>>(medicalRecords);

        return new PaginatedResponse<MedicalRecordListDto>(
            medicalRecordDtos,
            request.QueryParams.PageNumber,
            request.QueryParams.PageSize,
            totalCount);
    }
}

