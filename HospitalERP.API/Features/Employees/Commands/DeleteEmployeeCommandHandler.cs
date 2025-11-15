using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Employees.Commands;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteEmployeeCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeID == request.EmployeeID, cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.EmployeeID);
        }

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

