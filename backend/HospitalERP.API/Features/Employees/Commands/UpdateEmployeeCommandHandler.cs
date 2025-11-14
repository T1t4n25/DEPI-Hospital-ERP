using AutoMapper;
using HospitalERP.API.Common.Exceptions;
using HospitalERP.API.Data;
using HospitalERP.API.Features.Employees.Dtos;
using HospitalERP.API.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HospitalERP.API.Features.Employees.Commands;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailDto>
{
    private readonly HospitalDbContext _context;
    private readonly IMapper _mapper;

    public UpdateEmployeeCommandHandler(HospitalDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmployeeDetailDto> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeID == request.Dto.EmployeeID, cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.Dto.EmployeeID);
        }

        // Validate foreign keys
        var genderExists = await _context.Genders
            .AnyAsync(g => g.GenderID == request.Dto.GenderID, cancellationToken);
        if (!genderExists)
        {
            throw new NotFoundException(nameof(Gender), request.Dto.GenderID);
        }

        var roleExists = await _context.Roles
            .AnyAsync(r => r.RoleID == request.Dto.RoleID, cancellationToken);
        if (!roleExists)
        {
            throw new NotFoundException(nameof(Role), request.Dto.RoleID);
        }

        var departmentExists = await _context.Departments
            .AnyAsync(d => d.DepartmentID == request.Dto.DepartmentID, cancellationToken);
        if (!departmentExists)
        {
            throw new NotFoundException(nameof(Department), request.Dto.DepartmentID);
        }

        _mapper.Map(request.Dto, employee);
        await _context.SaveChangesAsync(cancellationToken);

        var updatedEmployee = await _context.Employees
            .Include(e => e.Gender)
            .Include(e => e.Role)
            .Include(e => e.Department)
            .FirstAsync(e => e.EmployeeID == employee.EmployeeID, cancellationToken);

        return _mapper.Map<EmployeeDetailDto>(updatedEmployee);
    }
}

