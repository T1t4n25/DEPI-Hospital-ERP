using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Patients.Queries;

public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, PatientDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetPatientByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientDetailDto> Handle(
        GetPatientByIdQuery request,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .AsNoTracking()
            .Where(p => p.PatientID == request.PatientID && !p.Deleted) // Filter out soft-deleted patients
            .Include(p => p.Gender)
            .Include(p => p.BloodType)
            .FirstOrDefaultAsync(cancellationToken);

        if (patient == null)
        {
            throw new NotFoundException(nameof(Patient), request.PatientID);
        }

        return _mapper.Map<PatientDetailDto>(patient);
    }
}

