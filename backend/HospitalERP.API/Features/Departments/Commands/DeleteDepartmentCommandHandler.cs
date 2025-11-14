using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Departments.Commands;

public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, Unit>
{
    private readonly HospitalDbContext _context;

    public DeleteDepartmentCommandHandler(HospitalDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentID == request.DepartmentID, cancellationToken);

        if (department == null)
        {
            throw new NotFoundException(nameof(Department), request.DepartmentID);
        }

        _context.Departments.Remove(department);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

