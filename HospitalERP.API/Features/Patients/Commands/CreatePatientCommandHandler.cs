using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Patients.Commands;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, PatientDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public CreatePatientCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientDetailDto> Handle(
        CreatePatientCommand request,
        CancellationToken cancellationToken)
    {
        // Validate foreign keys
        var genderExists = await _context.Genders
            .AnyAsync(g => g.GenderID == request.Dto.GenderID, cancellationToken);
        if (!genderExists)
        {
            throw new NotFoundException(nameof(Gender), request.Dto.GenderID);
        }

        var bloodTypeExists = await _context.BloodTypes
            .AnyAsync(bt => bt.BloodTypeID == request.Dto.BloodTypeID, cancellationToken);
        if (!bloodTypeExists)
        {
            throw new NotFoundException(nameof(BloodType), request.Dto.BloodTypeID);
        }

        var patient = _mapper.Map<Patient>(request.Dto);
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with includes for full detail
        var createdPatient = await _context.Patients
            .Include(p => p.Gender)
            .Include(p => p.BloodType)
            .FirstAsync(p => p.PatientID == patient.PatientID, cancellationToken);

        return _mapper.Map<PatientDetailDto>(createdPatient);
    }
}

