using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Patients.Commands;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeletePatientCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeletePatientCommand request,
        CancellationToken cancellationToken)
    {
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.PatientID == request.PatientID, cancellationToken);

        if (patient == null)
        {
            throw new NotFoundException(nameof(Patient), request.PatientID);
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
