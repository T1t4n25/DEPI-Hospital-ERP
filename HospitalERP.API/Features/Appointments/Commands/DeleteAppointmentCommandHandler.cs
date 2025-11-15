using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Appointments.Commands;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteAppointmentCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        var appointment = await _context.Appointments
            .FirstOrDefaultAsync(a => a.AppointmentID == request.AppointmentID, cancellationToken);

        if (appointment == null)
        {
            throw new NotFoundException(nameof(Appointment), request.AppointmentID);
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

