using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Patients.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Patients.Commands;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, PatientDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePatientCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PatientDetailDto> Handle(
        UpdatePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PatientID == request.Dto.PatientID, cancellationToken);

        if (patient == null)
        {
            throw new NotFoundException(nameof(Patient), request.Dto.PatientID);
        }

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

        _mapper.Map(request.Dto, patient);
        await _context.SaveChangesAsync(cancellationToken);

        // Reload with includes
        var updatedPatient = await _context.Patients
            .Include(p => p.Gender)
            .Include(p => p.BloodType)
            .FirstAsync(p => p.PatientID == patient.PatientID, cancellationToken);

        return _mapper.Map<PatientDetailDto>(updatedPatient);
    }
}

