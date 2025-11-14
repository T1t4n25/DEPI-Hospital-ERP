using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Departments.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Departments.Commands;

public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, DepartmentDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateDepartmentCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<DepartmentDetailDto> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d => d.DepartmentID == request.Dto.DepartmentID, cancellationToken);

        if (department == null)
        {
            throw new NotFoundException(nameof(Department), request.Dto.DepartmentID);
        }

        if (request.Dto.ManagerID.HasValue)
        {
            var employeeExists = await _context.Employees
                .AnyAsync(e => e.EmployeeID == request.Dto.ManagerID.Value, cancellationToken);
            if (!employeeExists)
            {
                throw new NotFoundException(nameof(Employee), request.Dto.ManagerID.Value);
            }
        }

        _mapper.Map(request.Dto, department);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedDepartment = await _context.Departments
            .Include(d => d.Manager)
            .FirstAsync(d => d.DepartmentID == department.DepartmentID, cancellationToken);

        return _mapper.Map<DepartmentDetailDto>(updatedDepartment);
    }
}

