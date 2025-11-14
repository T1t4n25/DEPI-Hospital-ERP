using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Medications.Commands;

public class DeleteMedicationCommandHandler : IRequestHandler<DeleteMedicationCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteMedicationCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteMedicationCommand request,
        CancellationToken cancellationToken)
    {
        var medication = await _context.Medications
            .FirstOrDefaultAsync(m => m.MedicationID == request.MedicationID, cancellationToken);

        if (medication == null)
        {
            throw new NotFoundException(nameof(Medication), request.MedicationID);
        }

        _context.Medications.Remove(medication);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

