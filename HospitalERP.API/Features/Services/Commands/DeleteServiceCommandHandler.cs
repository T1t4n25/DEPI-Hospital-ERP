using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Services.Commands;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteServiceCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteServiceCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _context.Services
            .FirstOrDefaultAsync(s => s.ServiceID == request.ServiceID, cancellationToken);

        if (service == null)
        {
            throw new NotFoundException(nameof(Service), request.ServiceID);
        }

        _context.Services.Remove(service);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

