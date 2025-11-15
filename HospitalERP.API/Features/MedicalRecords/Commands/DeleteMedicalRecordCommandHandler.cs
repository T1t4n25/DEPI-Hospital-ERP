using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.MedicalRecords.Commands;

public class DeleteMedicalRecordCommandHandler : IRequestHandler<DeleteMedicalRecordCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteMedicalRecordCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteMedicalRecordCommand request,
        CancellationToken cancellationToken)
    {
        var medicalRecord = await _context.MedicalRecords
            .FirstOrDefaultAsync(mr => mr.RecordID == request.RecordID, cancellationToken);

        if (medicalRecord == null)
        {
            throw new NotFoundException(nameof(MedicalRecord), request.RecordID);
        }

        _context.MedicalRecords.Remove(medicalRecord);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

