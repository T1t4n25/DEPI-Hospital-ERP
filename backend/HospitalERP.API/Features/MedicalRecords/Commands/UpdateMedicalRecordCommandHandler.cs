using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.MedicalRecords.Commands;

public class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, MedicalRecordDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateMedicalRecordCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicalRecordDetailDto> Handle(
        UpdateMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await _context.MedicalRecords
            .FirstOrDefaultAsync(mr => mr.RecordID == request.Dto.RecordID, cancellationToken);

        if (medicalRecord == null)
        {
            throw new NotFoundException(nameof(MedicalRecord), request.Dto.RecordID);
        }

        // Validate foreign keys
        var patientExists = await _context.Patients
            .AnyAsync(p => p.PatientID == request.Dto.PatientID, cancellationToken);
        if (!patientExists)
        {
            throw new NotFoundException(nameof(Patient), request.Dto.PatientID);
        }

        var doctorExists = await _context.Employees
            .AnyAsync(e => e.EmployeeID == request.Dto.DoctorID, cancellationToken);
        if (!doctorExists)
        {
            throw new NotFoundException(nameof(Employee), request.Dto.DoctorID);
        }

        var diagnosisExists = await _context.Diagnoses
            .AnyAsync(d => d.DiagnosesID == request.Dto.Diagnosesid, cancellationToken);
        if (!diagnosisExists)
        {
            throw new NotFoundException(nameof(Diagnosis), request.Dto.Diagnosesid);
        }

        _mapper.Map(request.Dto, medicalRecord);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedRecord = await _context.MedicalRecords
            .Include(mr => mr.Patient)
            .Include(mr => mr.Doctor)
            .Include(mr => mr.Diagnosis)
            .FirstAsync(mr => mr.RecordID == medicalRecord.RecordID, cancellationToken);

        return _mapper.Map<MedicalRecordDetailDto>(updatedRecord);
    }
}

