using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.MedicalRecords.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.MedicalRecords.Queries;

public class GetMedicalRecordByIdQueryHandler : IRequestHandler<GetMedicalRecordByIdQuery, MedicalRecordDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public GetMedicalRecordByIdQueryHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MedicalRecordDetailDto> Handle(
        GetMedicalRecordByIdQuery request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await _context.MedicalRecords
            .AsNoTracking()
            .Include(mr => mr.Patient)
            .Include(mr => mr.Doctor)
            .Include(mr => mr.Diagnosis)
            .FirstOrDefaultAsync(mr => mr.RecordID == request.RecordID, cancellationToken);

        if (medicalRecord == null)
        {
            throw new NotFoundException(nameof(MedicalRecord), request.RecordID);
        }

        return _mapper.Map<MedicalRecordDetailDto>(medicalRecord);
    }
}

